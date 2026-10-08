// Read-only route, asset and responsive smoke checks; requires the API, UI and VerifyHost.
const { chromium } = require('playwright');
const fs = require('fs');
const path = require('path');
const assert = require('node:assert/strict');
const output = path.resolve('artifacts/flix-verification');
fs.mkdirSync(output, { recursive: true });
const routes = [
 ['index.html','/'],['catalog.html','/catalog'],['category.html','/category'],
 ['about.html','/about'],['contacts.html','/contacts'],['interview.html','/interview'],
 ['privacy.html','/privacy'],['main/details.html','/details/1'],
 ['profile.html','/profile',true],
 ['admin/index.html','/admin/flix',true],['admin/catalog.html','/admin/flix/catalog',true],
 ['admin/users.html','/admin/flix/users',true],['admin/comments.html','/admin/flix/comments',true],
 ['admin/reviews.html','/admin/flix/reviews',true],['admin/add-item.html','/admin/flix/add-item',true],
 ['admin/edit-user.html','/admin/flix/users',true],
 ['admin/signin.html','/signin'],['admin/signup.html','/signup'],['admin/forgot.html','/forgot'],
 ['admin/404.html','/admin/404.html'], ['series-extra','/category?kind=series']
];
const selectedTemplates = process.argv.slice(2);
const templateRoot = path.resolve('Frontends/Movie.Api.UI/wwwroot/FlixTheme');
const actualTemplates = fs.readdirSync(templateRoot, {recursive:true}).filter(file=>file.endsWith('.html')).map(file=>file.replaceAll('\\','/')).sort();
assert.deepEqual(routes.map(r=>r[0]).filter(file=>file.endsWith('.html')).sort(), actualTemplates, 'Every bundled HTML page must have a route check');
(async () => {
 const browser = await chromium.launch({executablePath: process.env.FLIX_BROWSER || 'C:/Program Files/Google/Chrome/Application/chrome.exe', headless: true});
 const results = [];
 for (const width of [1440,390]) {
  const context = await browser.newContext({viewport:{width,height:1000}, reducedMotion:'reduce'});
  for (const [template,route,protectedPage] of routes) {
   if(selectedTemplates.length && !selectedTemplates.includes(template)) continue;
   const page = await context.newPage();
   const errors = [], failed = [];
   page.on('pageerror',e=>errors.push(e.message));
   page.on('response',r=>{if(r.status()>=400 && !r.url().endsWith('/404.html')) failed.push({url:r.url(),status:r.status()});});
   page.on('requestfailed',r=>failed.push({url:r.url(),error:r.failure()?.errorText}));
   const base = protectedPage ? 'http://127.0.0.1:5019' : 'http://localhost:5009';
   let target=route;
   if(template==='admin/edit-user.html') {
    const users = await (await fetch('http://localhost:5114/api/Users')).json();
    target='/admin/flix/edit-user/'+users[0].id;
   }
   try {
    const response=await page.goto(base+target,{waitUntil:'networkidle',timeout:30000});
    await page.evaluate(()=>document.fonts.ready);
    await page.evaluate(async () => {
      document.querySelectorAll('img').forEach(img => img.loading = 'eager');
      await Promise.race([Promise.all([...document.images].map(img => img.decode().catch(() => {}))), new Promise(resolve => setTimeout(resolve, 8000))]);
    });
    const metrics=await page.evaluate(()=>({
      title:document.title, h1:document.querySelector('h1,h2')?.textContent?.trim(),
      overflow:document.documentElement.scrollWidth>innerWidth,
      brokenImages:[...document.images].filter(i=>i.complete && i.naturalWidth===0).map(i=>i.getAttribute('src')),
      cards:document.querySelectorAll('.card').length,
      styles:[...document.querySelectorAll('link[rel=stylesheet]')].map(x=>x.getAttribute('href')),
      scripts:[...document.scripts].filter(x=>x.src).map(x=>x.getAttribute('src'))
    }));
    const name=template.replaceAll('/','-').replace('.html','')+'-'+width;
    await page.screenshot({path:path.join(output,name+'.png'),fullPage:true});
    results.push({template,route:target,protectedPage:!!protectedPage,width,status:response.status(),...metrics,errors,failed});
    console.log(JSON.stringify({template,width,status:response.status(),overflow:metrics.overflow,errors:errors.length,failed:failed.length}));
   } catch(e) { results.push({template,route:target,width,error:e.message}); console.log(JSON.stringify({template,width,error:e.message})); }
   await page.close();
  }
  await context.close();
 }
 const retained = selectedTemplates.length && fs.existsSync(path.join(output,'routes.json')) ? JSON.parse(fs.readFileSync(path.join(output,'routes.json'))).filter(r=>!selectedTemplates.includes(r.template)) : [];
 fs.writeFileSync(path.join(output,'routes.json'),JSON.stringify([...retained,...results],null,2));
 await browser.close();
 if(results.some(r=>r.error || r.status !== (r.template==='admin/404.html'?404:200) || r.overflow || r.errors.length || r.brokenImages.length || r.failed.some(f=>/^http:\/\/(localhost|127\.0\.0\.1):/.test(f.url)))) process.exitCode=1;
})().catch(e=>{console.error(e);process.exitCode=1;});
