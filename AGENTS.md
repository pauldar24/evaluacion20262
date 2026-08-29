# AGENTS.md

## Project

Single-project ASP.NET Core MVC web app (`.NET 10`, `net10.0`) created from the stock `dotnet new mvc` template. No solution file, no test project, no CI, no linter, no README. Features are added incrementally on top of this plain template.

Git: only branch `develop` exists (single commit). Commit work there.

## Commands

- `dotnet build` — compiles with 0 warnings/errors (verified working)
- `dotnet run` — dev server; profiles in `Properties/launchSettings.json`: HTTP `http://localhost:5011`, HTTPS `https://localhost:7065`
- `dotnet run --launch-profile http` — force the plain-HTTP profile
- No test framework present; do not assume one exists.

Note: this machine's dotnet CLI emits Spanish output — build results show `Errores` (errors) and `Advertencia(s)` (warnings).

## Sharp edges

- `Program.cs` uses `MapStaticAssets()` / `.WithStaticAssets()`, not `UseStaticFiles()`. Static files under `wwwroot/` are fingerprinted/cached — new files need a rebuild before they are served.
- The `~/evaluacion20262.styles.css` tag in `_Layout.cshtml` is a **build-generated** scoped-CSS bundle compiled from `*.cshtml.css` files (e.g. `Views/Shared/_Layout.cshtml.css`). Never edit the generated bundle; edit the `.cshtml.css` source instead.
- `wwwroot/lib/*` are vendored upstream libraries (bootstrap, jquery, validation) — treat as generated, don't edit.
- Views rely on tag helpers via `_ViewImports.cshtml` (`@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers`); keep using them in new views.
- Routing is convention-based: `{controller=Home}/{action=Index}/{id?}` defined in `Program.cs`.