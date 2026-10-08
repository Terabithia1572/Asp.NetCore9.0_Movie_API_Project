# FlixTheme integration

Verified on 2026-10-08. The actual source is `Frontends/Movie.Api.UI/wwwroot/FlixTheme`: **20 HTML files**, comprising 9 public pages and 11 admin/authentication pages. The original HTML, CSS, JavaScript and media files remain unchanged. Links in the vendor HTML to absent variants such as `index2.html`, `index3.html` and live pages do not represent files in this bundle.

## Implementation

The application now uses the original public, admin and authentication layouts separately. Shared headers, footers, navigation, cards, filters, pagination and carousels are Razor partials. `FlixCategories` and `FlixLatest` View Components load their own data. Strongly typed models and `FlixCatalogService` keep data access out of views and reuse the existing API, DTOs and Identity store.

The old integration mixed incompatible layout assets, used incorrect relative paths and an arbitrary pink CSS override, and linked to static HTML rather than records. Styles and scripts now use root-relative paths and the vendor dependency order. Public carousel/select/player dependencies are conditional. A guarded copy of the vendor initialization script supports pages without those plugins; dropdown parents and click propagation are corrected. Only integration needs such as validation, poster sizing, missing images and unavailable controls use the additional stylesheet.

Movies and series support search, genre/type/year filters, sorting, pagination, published-state filtering and record-specific detail links. Details include existing cast, tags, reviews, seasons and episodes. Legacy public routes redirect into these pages; existing management features remain accessible under the Admin area and require the Admin role. The API address is configured centrally through `MovieApi:BaseUrl`.

Sign-in validates existing Identity password hashes and roles; registration, profile/password updates, favorites, item creation and user editing call real existing persistence functionality. The new forms use server validation and antiforgery tokens. Favorite ownership comes from the signed-in identity. Old fabricated identity cookies are not accepted under the new cookie name.

## Complete page checklist

All view paths below are relative to `Frontends/Movie.Api.UI/`. **D/M passed** means the route rendered at 1440×1000 and 390×1000, without JavaScript exceptions, horizontal page overflow, broken rendered images or failed local assets. Each template was also compared with its original at both widths. Protected pages were rendered using the read-only verification host described below; that is not a successful credential-login test. A checked item confirms UI integration and the stated read checks, not an unsupported service or an unexecuted write.

- [x] `index.html` → `Views/Flix/Index.cshtml` → `/` (alias `/index.html`). Existing movies, series, genres, ratings, featured titles and carousel. D/M passed.
- [x] `catalog.html` → `Views/Flix/Catalog.cshtml` → `/catalog` (alias `/catalog.html`). Existing catalog and genre counts; record links and filter navigation. D/M passed.
- [x] `category.html` → `Views/Flix/Category.cshtml` → `/category`, `/category/{id}` (alias `/category.html`). Existing catalog with search, type/genre/year/sort/page parameters. D/M passed; search, empty state, invalid category and pagination checked.
- [x] `main/details.html` → `Views/Flix/Details.cshtml` → `/details/{id}` and `/series/{id}`. Movies/series, cast, tags, reviews, related titles, seasons/episodes and favorite state. `/details.html?id=…` and `/main/details.html?id=…` redirect to a movie; without an ID they open the movie list. D/M passed; missing IDs and actual series seasons checked. Playback requires media support.
- [x] `about.html` → `Views/Flix/About.cshtml` → `/about` (alias `/about.html`). Static theme content; subscription controls explicitly unavailable. D/M passed; subscription modal checked.
- [x] `contacts.html` → `Views/Flix/Contacts.cshtml` → `/contacts` (alias `/contacts.html`). Original contact UI; no configured delivery backend, no success simulated. D/M passed.
- [x] `interview.html` → `Views/Flix/Interview.cshtml` → `/interview` (alias `/interview.html`). Labelled template editorial preview, including the original embedded YouTube video, with real latest-title sidebar. Editorial publishing/comments/likes are unavailable. D/M passed.
- [x] `privacy.html` → `Views/Flix/Privacy.cshtml` → `/privacy` (alias `/privacy.html`). Static template policy content. D/M passed; content still requires the site's own policy text before publication.
- [x] `profile.html` → `Views/Flix/Profile.cshtml` → `/profile` (aliases `/profile.html`, `/Profile/Index`). Current Identity user, own favorites/reviews and latest titles; profile/password forms. D/M passed in read-only host; tabs and form tokens checked. Production route requires login.
- [x] `admin/index.html` → `Views/FlixAdmin/Index.cshtml` → `/admin/flix` (alias `/admin/index.html`). Real dashboard counts, catalog, users and recent reviews. D/M passed in read-only host; mobile sidebar checked.
- [x] `admin/catalog.html` → `Views/FlixAdmin/Catalog.cshtml` → `/admin/flix/catalog` (alias `/admin/catalog.html`). Published/unpublished movies and series with search, sorting, pagination and existing editor links. D/M passed in read-only host.
- [x] `admin/users.html` → `Views/FlixAdmin/Users.cshtml` → `/admin/flix/users` (alias `/admin/users.html`). Existing Identity users, search, pagination and contextual edit links. D/M passed in read-only host; empty search checked.
- [x] `admin/comments.html` → `Views/FlixAdmin/Comments.cshtml` → `/admin/flix/comments` (alias `/admin/comments.html`). Existing review text, explicitly identified as such because there is no separate comments model. Search, pagination and read modal; moderation unavailable. D/M passed in read-only host.
- [x] `admin/reviews.html` → `Views/FlixAdmin/Reviews.cshtml` → `/admin/flix/reviews` (alias `/admin/reviews.html`). Existing movie/series reviews, ratings, dates, search, pagination and read modal. D/M passed in read-only host; modal checked. Moderation unavailable.
- [x] `admin/add-item.html` → `Views/FlixAdmin/AddItem.cshtml` → `/admin/flix/add-item` (alias `/admin/add-item.html`). Existing category/cast/tag choices and real movie/series create DTOs. Unsupported media controls remain visible and disabled. D/M passed in read-only host; saving not executed.
- [x] `admin/edit-user.html` → `Views/FlixAdmin/EditUser.cshtml` → `/admin/flix/edit-user/{id}`. `/admin/edit-user.html?id=…` opens that record; no ID redirects to users. Existing Identity profile, status and reviews. D/M passed in read-only host; invalid ID checked. Updating not executed; role/subscription/moderation controls unavailable.
- [x] `admin/signin.html` → `Views/Flix/SignIn.cshtml` → `/signin` (aliases `/admin/signin.html`, `/Login/SignIn`). Real Identity credential validation. D/M passed; invalid credentials and missing antiforgery token rejected. No valid password was supplied for a successful login test.
- [x] `admin/signup.html` → `Views/Flix/SignUp.cshtml` → `/signup` (aliases `/admin/signup.html`, `/Register/SignUp`). Identity registration with required name fields, password confirmation and privacy acceptance. D/M passed; missing antiforgery token rejected. No user created during verification.
- [x] `admin/forgot.html` → `Views/Flix/Forgot.cshtml` → `/forgot` (alias `/admin/forgot.html`). Original reset UI with explicit unavailable message because reset-token/email delivery is not configured. D/M passed.
- [x] `admin/404.html` → `Views/FlixAdmin/NotFound.cshtml` → `/admin/404.html`; shared status handling also uses `/Error/NotFound404`. D/M passed with HTTP 404. Re-executed errors preserve their original HTTP status, including antiforgery HTTP 400.

## Database preservation and mapping corrections

Startup was inspected before execution: neither application runs database creation, migration or seeding. No migration was generated/applied and no records were reset, deleted, inserted or edited during verification. The existing database was reachable with its existing configuration; no credentials were copied into this report or new configuration.

Read-only inspection found that `Reviews.UserRating` is `tinyint`, `MovieID` is nullable, and existing series reviews use `SeriesID`. The previous CLR mapping threw casting/null exceptions. The existing entity, DTOs and EF mapping now match those columns; the dashboard and review API can read both movie and series reviews. The movie query also now includes its existing `CategoryID`, fixing genre filtering. These are code mapping corrections, **not schema changes**. The existing migration snapshot was left untouched; any later migration must account for the already-existing database shape rather than reapplying these differences blindly.

## Verification evidence

- Solution build succeeded with zero errors. A clean compilation earlier in the task reported 109 warnings; the final incremental solution build reported 6 warnings. Existing nullable/obsolete warnings remain outside this integration. The verification host also builds successfully.
- `tools/verify-flix.cjs` checks the discovered HTML inventory against the route list. It recorded 42 renders: 20 templates × 2 widths plus the series catalog at both widths. Expected HTTP statuses passed. No JS exceptions, horizontal overflow, failed local assets or broken rendered images remained.
- `tools/verify-flix-references.cjs` visits all 20 original templates and their integrated counterparts at both widths. It compares typography, spacing, colors, radii and layout properties on 18 shared selectors, checks unresolved placeholder/fragment links and duplicate IDs, and saves screenshots. All 40 comparisons had zero differences in the compared properties, zero unresolved links and zero duplicate IDs. Side-by-side screenshot sheets were also visually inspected for all 20 pages. This is a structural/style comparison, not a claim of pixel-identical content: database records, required input fields and explicit unsupported states naturally differ from demo content.
- `tools/verify-flix-interactions.cjs`: 19 checks passed, covering protected-route redirects, antiforgery rejection, invalid credentials, real searches/filtering/pagination, empty results, sorting, dropdowns, carousel, unavailable-feature modal, mobile navigation/search, profile tabs, admin sidebar/review modal, admin sorting preserved across styled pagination, invalid records and series seasons. It does not perform successful writes.
- Some stored external IMDb poster URLs return HTTP 404. Local fallback posters render correctly. The URLs in the database were preserved. Those external request failures remain visible in the raw results and are not counted as successful downloads.
- An actual API interruption was checked: home and movie details returned HTTP 503, while About remained HTTP 200. The API was restarted afterward. Results are in `api-unavailable.json`.
- Screenshots and raw `routes.json`, `references.json`, `interactions.json` are under ignored `artifacts/flix-verification/`. They are local evidence and may contain existing profile data; they are not committed or published.

The separate `tools/FlixTheme.VerifyHost` uses an existing user ID with an Admin claim solely to render protected views. It binds to loopback on port 5019, rejects non-GET/HEAD requests, and blocks legacy controllers and deletion paths. It is not part of the production app or solution and must not be deployed. Main-app authorization redirects were tested separately. Successful login, registration, favorites writes, profile/password changes, item creation and user updates were not executed against existing records.

## Missing backend functionality

- Video/trailer/download sources, media uploads/storage, gallery, quality, age certification and country metadata have no matching integrated backend fields/services. The UI exposes unavailable states instead of unrelated playable demo content.
- Payments, subscriptions and balance top-ups have no connected provider or domain implementation.
- Contact/newsletter delivery, social login and password-reset email/token delivery are unconfigured.
- Review creation/moderation and separate editorial comments/likes have no corresponding implemented endpoints. Existing reviews are readable; unavailable actions do not report success.
- Editorial/about/contact/privacy demo copy is retained as template content; it is not a database-backed CMS or a verified business/legal policy.
- No schema changes are proposed or needed for the completed integration. Adding the unsupported services would be a separate feature scope.

## Reproduce locally

Use the existing SQL Server configuration and .NET SDK. From the repository root:

```powershell
dotnet build Asp.NetCore9.0_Movie_API_Project.sln --artifacts-path artifacts/build -v:q -clp:ErrorsOnly
dotnet build tools/FlixTheme.VerifyHost/FlixTheme.VerifyHost.csproj --artifacts-path artifacts/build -v:q -clp:ErrorsOnly
```

Run the API DLL from `Presentation/Movie.Api.WebApi` with `--urls http://localhost:5114`, and the UI DLL from `Frontends/Movie.Api.UI` with `--urls http://localhost:5009`, both using `ASPNETCORE_ENVIRONMENT=Development`. The DLLs are under `artifacts/build/bin/<project>/debug/`. The UI API base defaults to the API address above and can be overridden with `MovieApi__BaseUrl`.

For optional read-only protected-page verification, run `dotnet artifacts/build/bin/FlixTheme.VerifyHost/debug/FlixTheme.VerifyHost.dll` from the root using the documented artifacts build path. With Playwright available to Node, run the three `tools/verify-flix*.cjs` scripts in order: routes, references, interactions. `FLIX_BROWSER` can override the default locally installed Chrome executable. Stop the verification host after checking; normal browsing uses `http://localhost:5009`.

Suggested commit message: `feat: integrate all FlixTheme pages with existing movie data`

No commit or push was performed.
