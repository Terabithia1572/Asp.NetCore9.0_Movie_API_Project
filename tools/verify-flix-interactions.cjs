const {chromium}=require('playwright');
const assert=require('node:assert/strict');
const fs=require('fs');
const output='artifacts/flix-verification';
fs.mkdirSync(output,{recursive:true});
(async()=>{
 const browser=await chromium.launch({executablePath:process.env.FLIX_BROWSER||'C:/Program Files/Google/Chrome/Application/chrome.exe',headless:true});
 const results=[];
 const page=await browser.newPage({viewport:{width:1440,height:1000}});
 async function test(name,fn){try{await fn();results.push({name,pass:true});}catch(e){results.push({name,pass:false,error:e.message});}console.log(JSON.stringify(results.at(-1)));}
 const base='http://localhost:5009', auth='http://127.0.0.1:5019';
 await test('Protected routes require sign-in',async()=>{
  for(const route of ['/profile','/admin/flix','/admin/flix/users','/admin/flix/add-item']){
   const r=await fetch(base+route,{redirect:'manual'});assert.equal(r.status,302);assert.ok(r.headers.get('location').includes('/Login/SignIn'));
  }
 });
 await test('POSTs without antiforgery tokens rejected',async()=>{
  for(const route of ['/signin','/signup']) assert.equal((await fetch(base+route,{method:'POST',body:''})).status,400);
 });
 await test('Invalid credentials do not create a session',async()=>{
  await page.goto(base+'/signin');
  await page.locator('[name=Username]').fill('flix-verification-account-does-not-exist');
  await page.locator('[name=Password]').fill('IntentionallyInvalidVerification');
  await Promise.all([page.waitForURL(base+'/signin'),page.locator('button[type=submit]').click()]);
  await page.locator('.validation-summary-errors').waitFor();
  assert.ok((await page.locator('.validation-summary-errors').innerText()).includes('Invalid credentials'));
  assert.ok(!(await page.context().cookies()).some(c=>c.name==='FlixTV.Identity'));
 });
 await test('Search finds existing titles',async()=>{
  await page.goto(base+'/category?query=Oppenheimer&kind=movie');
  const titles=await page.locator('.catalog .card__title').allTextContents();
  assert.deepEqual(titles,['Oppenheimer']);
 });
 await test('Empty search and page clamping',async()=>{
  await page.goto(base+'/category?query=flix-no-such-title&page=-900');
  assert.ok((await page.locator('body').innerText()).includes('No titles match'));
  assert.equal(await page.locator('.catalog .card').count(),0);
 });
 await test('Category query returns only matching database records',async()=>{
  const movies=await (await fetch('http://localhost:5114/api/Movies')).json();
  const category=movies[0].categoryID;assert.ok(category>0);
  await page.goto(base+'/category/'+category+'?kind=movie');
  const links=await page.locator('.catalog .card__cover').evaluateAll(a=>a.map(x=>x.getAttribute('href')));
  assert.ok(links.length>0);
  for(const link of links) assert.equal(movies.find(m=>'/details/'+m.movieID===link)?.categoryID,category);
 });
 await test('Pagination preserves filtering and changes records',async()=>{
  await page.goto(base+'/category?kind=movie');
  const first=await page.locator('.catalog .card__cover').first().getAttribute('href');
  await page.locator('.catalog__paginator a').filter({hasText:/^2$/}).click();
  assert.ok(page.url().includes('kind=movie'));assert.ok(page.url().includes('page=2'));
  assert.notEqual(await page.locator('.catalog .card__cover').first().getAttribute('href'),first);
 });
 await test('Sort radio submits a real sorted query',async()=>{
  await page.goto(base+'/category?kind=movie');
  await page.locator('label[for=sort-title]').click();
  await page.waitForURL(/sort=title/);
  assert.ok(page.url().includes('kind=movie'));
 });
 await test('Genre and page dropdowns open and close',async()=>{
  await page.goto(base+'/');
  for(const name of ['genres','pages']) {
   await page.locator('#'+name+'-toggle').click();
   assert.ok(await page.locator('#'+name+'-menu').evaluate(e=>e.classList.contains('show')));
   await page.locator('#'+name+'-toggle').click();
   assert.ok(await page.locator('#'+name+'-menu').evaluate(e=>!e.classList.contains('show')));
  }
 });
 await test('Home carousel navigation changes its active title',async()=>{
  await page.goto(base+'/');
  const before=await page.locator('#flixtv-hero .owl-item.center h2').textContent();
  await page.locator('.home__nav--next').click();
  await page.waitForFunction(previous=>document.querySelector('#flixtv-hero .owl-item.center h2')?.textContent!==previous,before);
 });
 await test('Unavailable subscription opens honest modal',async()=>{
  await page.goto(base+'/about');
  await page.locator('.plan__btn').first().click();
  await page.locator('.mfp-content #feature-unavailable').waitFor();
  assert.ok((await page.locator('#unavailable-message').innerText()).includes('no connected service'));
  await page.locator('.modal__close').click();
 });
 await test('Mobile navigation and search open and close',async()=>{
  await page.setViewportSize({width:390,height:844});await page.goto(base+'/');
  await page.locator('.header__menu').click();assert.ok(await page.locator('.header__nav--active').count());
  await page.locator('.header__menu').click();assert.equal(await page.locator('.header__nav--active').count(),0);
  await page.locator('.header__search').click();assert.ok(await page.locator('.header__form--active').count());
  await page.locator('.header__form-close').click();assert.equal(await page.locator('.header__form--active').count(),0);
 });
 await test('Read-only profile tabs render actual favorites and settings',async()=>{
  await page.goto(auth+'/profile');
  await page.locator('.profile__tabs a[href="#tab-2"]').click();await page.waitForFunction(()=>document.querySelector('#tab-2').classList.contains('active'));
  await page.locator('.profile__tabs a[href="#tab-3"]').click();await page.locator('[name=CurrentPassword]').waitFor();
  assert.equal(await page.locator('form[action="/Profile/ChangePassword"] input[name=__RequestVerificationToken]').count(),1);
 });
 await test('Admin mobile navigation opens',async()=>{
  await page.goto(auth+'/admin/flix');await page.locator('.header__btn').click();assert.ok(await page.locator('.sidebar--active').count());
 });
 await test('Admin review modal shows stored text',async()=>{
  await page.setViewportSize({width:1440,height:1000});await page.goto(auth+'/admin/flix/reviews');
  await page.locator('a.open-modal[href^="#review-"]').first().click();await page.locator('.mfp-content .modal').waitFor();
  assert.ok((await page.locator('.mfp-content .modal__text').first().innerText()).length>0);
  await page.locator('.mfp-content .modal__btn--dismiss').click();
 });
 await test('Admin filters and invalid record IDs',async()=>{
  await page.goto(auth+'/admin/flix/users?query=flix-no-such-user');
  assert.ok((await page.locator('body').innerText()).includes('No matching records'));
  assert.equal((await fetch(auth+'/admin/flix/edit-user/flix-no-such-user')).status,404);
  assert.equal((await fetch(base+'/details/2147483647')).status,404);
  assert.equal((await fetch(base+'/category/2147483647')).status,404);
 });
 await test('Admin sorting and styled pagination retain the sort',async()=>{
  await page.goto(auth+'/admin/flix/catalog');
  await page.locator('#filter-sort').click();await page.locator('[data-sort=title]').click();await page.waitForURL(/sort=title/);
  await page.locator('.paginator__paginator a').filter({hasText:/^2$/}).click();
  assert.ok(page.url().includes('sort=title'));assert.ok(page.url().includes('page=2'));
  assert.equal(await page.locator('.paginator__paginator a').first().evaluate(e=>getComputedStyle(e).width),'30px');
 });
 await test('Series details include actual seasons and episodes',async()=>{
  const series=await (await fetch('http://localhost:5114/api/Series')).json();
  const item=series.find(s=>s.seriesStatus);
  const r=await page.goto(base+'/series/'+item.seriesID);assert.equal(r.status(),200);
  assert.equal((await page.locator('h1').textContent()).trim(),item.seriesTitle);
  const seasons=await (await fetch('http://localhost:5114/api/Seasons/series/'+item.seriesID)).json();
  assert.equal(await page.locator('.series-wrap').count(),seasons.length);
 });
 await test('Verification host refuses all writes and legacy controllers',async()=>{
  assert.equal((await fetch(auth+'/admin/flix/add-item',{method:'POST'})).status,405);
  assert.equal((await fetch(auth+'/Admin/AdminMovie/DeleteMovie/1')).status,405);
 });
 fs.writeFileSync(output+'/interactions.json',JSON.stringify(results,null,2));
 await browser.close();
 if(results.some(r=>!r.pass))process.exitCode=1;
})().catch(e=>{console.error(e);process.exitCode=1;});
