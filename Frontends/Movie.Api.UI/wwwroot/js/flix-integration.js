(function () {
    "use strict";
    function feedback(message) { var box = document.getElementById("flix-feedback"); if (box) { box.textContent = message; box.hidden = false; } }
    function unavailable(message) {
        var target = document.getElementById("unavailable-message");
        if (target) target.textContent = message || "This feature needs a connected service. No action has been submitted.";
        if (window.jQuery && $.magnificPopup) $.magnificPopup.open({items: {src: "#feature-unavailable"}, type: "inline"});
    }
    document.addEventListener("click", async function (event) {
        var sort = event.target.closest("[data-sort]");
        if (sort) {
            var url = new URL(location.href);
            url.searchParams.set("sort", sort.dataset.sort);
            url.searchParams.delete("page");
            location.href = url.toString();
            return;
        }
        var unsupported = event.target.closest("[data-unavailable]");
        if (unsupported) { event.preventDefault(); unavailable(unsupported.dataset.unavailable); return; }
        var favorite = event.target.closest("[data-favorite]");
        if (!favorite) return;
        event.preventDefault();
        var token = document.querySelector('#flix-antiforgery input');
        favorite.disabled = true;
        try {
            var response = await fetch("/Favorite/Toggle" + (favorite.dataset.kind === "series" ? "Series" : "Movie") + "Favorite?" + (favorite.dataset.kind === "series" ? "seriesId=" : "movieId=") + favorite.dataset.favorite,
                {method: "POST", headers: {"Content-Type": "application/json", "RequestVerificationToken": token.value}, body: "{}"});
            if (response.redirected || response.status === 401) { location.href = "/signin?returnUrl=" + encodeURIComponent(location.pathname + location.search); return; }
            if (!response.ok) throw new Error("Unable to update favorites.");
            var result = await response.json();
            if (!result.success) throw new Error(result.message || "Unable to update favorites.");
            favorite.classList.toggle("active", result.isFavorited);
            favorite.setAttribute("aria-pressed", String(result.isFavorited));
            feedback(result.isFavorited ? "Added to favorites." : "Removed from favorites.");
        } catch (error) { feedback(error.message); }
        finally { favorite.disabled = false; }
    });
    document.addEventListener("keydown", function (event) {
        if (event.target.matches("[data-sort]") && (event.key === "Enter" || event.key === " ")) { event.preventDefault(); event.target.click(); }
    });
    document.addEventListener("error", function (event) {
        if (event.target.tagName === "IMG" && !event.target.src.endsWith("/images/poster-placeholder.svg")) event.target.src = "/images/poster-placeholder.svg";
    }, true);
    document.querySelectorAll("img").forEach(function (img) {
        if (img.complete && !img.naturalWidth) img.src = "/images/poster-placeholder.svg";
    });
    document.querySelectorAll(".card__add:not([data-favorite]), .subscribe button, .comments__form button, .comments__rate button, .comments__actions button, .plan__btn").forEach(function (button) {
        button.setAttribute("data-unavailable", "This template preview feature has no connected service.");
    });
    document.querySelectorAll("[data-filter]").forEach(function (form) {
        form.addEventListener("change", function () { form.submit(); });
        if (window.jQuery) $(form).find("select").on("select2:select", function () { form.submit(); });
    });
    if (/^#tab-[123]$/.test(location.hash) && window.jQuery) {
        $('.profile__tabs a[href="' + location.hash + '"]').tab("show");
    }
})();
