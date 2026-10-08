const {chromium}=require('playwright');
const fs=require('fs'),path=require('path');
const root=path.resolve('Frontends/Movie.Api.UI/wwwroot/FlixTheme');
const output=path.resolve('artifacts/flix-verification');
const selectedTemplates=process.argv.slice(2);
const selectors=['body','.header__content','.header__logo','.header__nav-link','.section__title','.home__card','.card','.card__cover','.card__title','.catalog__nav','.category','.article__content h1','.main__title','.sidebar','.stats','.sign__form','.page-404__title'];
const properties=['fontFamily','fontSize','fontWeight','lineHeight','color','backgroundColor','borderRadius','paddingTop','paddingBottom','marginTop','marginBottom','display','flexDirection'];
(async()=>{
 const browser=await chromium.launch({executablePath:process.env.FLIX_BROWSER||'C:/Program Files/Google/Chrome/Application/chrome.exe',headless:true});
 const routes=JSON.parse(fs.readFileSync(output+'/routes.json'));
 const results=[];
 for(const route of routes.filter(r=>r.template.endsWith('.html'))){
  if(selectedTemplates.length && !selectedTemplates.includes(route.template)) continue;
  const page=await browser.newPage({viewport:{width:route.width,height:1000}});
  const html=fs.readFileSync(path.join(root,route.template),'utf8');
  // Normalize the nested details file's relative base in memory only. Original files stay untouched.
  const base='/FlixTheme/'+(route.template.startsWith('admin/')?'admin/':'');
  await page.route('**/reference-page',r=>r.fulfill({contentType:'text/html',body:html.replace('<head>','<head><base href="'+base+'">')}));
  const errors=[];
  page.on('pageerror',e=>errors.push(e.message));
  await page.goto('http://localhost:5009/reference-page',{waitUntil:'networkidle',timeout:30000});
  const before=await page.evaluate(({selectors,properties})=>Object.fromEntries(selectors.map(selector=>{
    const el=document.querySelector(selector);return [selector,el?Object.fromEntries(properties.map(prop=>[prop,getComputedStyle(el)[prop]])):null];
  })),{selectors,properties});
  await page.screenshot({path:path.join(output,'reference-'+route.template.replaceAll('/','-').replace('.html','')+'-'+route.width+'.png'),fullPage:true});
  const baseApp=route.protectedPage?'http://127.0.0.1:5019':'http://localhost:5009';
  await page.goto(baseApp+route.route,{waitUntil:'networkidle',timeout:30000});
  const after=await page.evaluate(({selectors,properties})=>Object.fromEntries(selectors.map(selector=>{
    const el=document.querySelector(selector);return [selector,el?Object.fromEntries(properties.map(prop=>[prop,getComputedStyle(el)[prop]])):null];
  })),{selectors,properties});
  const differences=[];
  for(const selector of selectors) if(before[selector]&&after[selector]) for(const prop of properties) if(before[selector][prop]!==after[selector][prop]) differences.push({selector,property:prop,original:before[selector][prop],integrated:after[selector][prop]});
  const links=await page.evaluate(()=>[...document.querySelectorAll('a[href]')].map(a=>({href:a.getAttribute('href'),label:a.textContent.trim()})).filter(a=>a.href==='#'||(a.href.startsWith('#')&&a.href.length>1&&!document.getElementById(a.href.slice(1)))||(/^[^/#]+\.html$/.test(a.href))));
  const duplicates=await page.evaluate(()=>{const ids=[...document.querySelectorAll('[id]')].map(e=>e.id);return [...new Set(ids.filter((id,i)=>ids.indexOf(id)!==i))];});
  results.push({template:route.template,width:route.width,differences,unresolvedLinks:links,duplicateIds:duplicates,referenceErrors:errors});
  console.log(JSON.stringify({template:route.template,width:route.width,differences:differences.length,links:links.length,duplicateIds:duplicates.length}));
  await page.close();
 }
 const retained=selectedTemplates.length && fs.existsSync(output+'/references.json') ? JSON.parse(fs.readFileSync(output+'/references.json')).filter(r=>!selectedTemplates.includes(r.template)) : [];
 fs.writeFileSync(output+'/references.json',JSON.stringify([...retained,...results],null,2));
 await browser.close();
 if(results.some(r=>r.unresolvedLinks.length || r.duplicateIds.length)) process.exitCode=1;
})().catch(e=>{console.error(e);process.exitCode=1;});
