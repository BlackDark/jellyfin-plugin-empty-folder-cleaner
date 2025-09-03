const hideEmptyFoldersConfigurationPage = {
    pluginUniqueId: "e3a1b2c4-1234-5678-9abc-def012345678",

    loadConfiguration: (page) => {
        Dashboard.showLoadingMsg();
        ApiClient.getPluginConfiguration(hideEmptyFoldersConfigurationPage.pluginUniqueId).then(function (config) {
            page.querySelector("#scanFolderPath").value = config.ScanFolderPath || "";
            page.querySelector("#scanIntervalMinutes").value = config.ScanIntervalMinutes || 60;
            page.querySelector("#videoExtensions").value = config.VideoExtensions || "avi,mp4,mkv,mov,wmv,flv,webm,m4v,mpg,mpeg,ts,mts,m2ts,3gp,3g2,f4v";
            Dashboard.hideLoadingMsg();
        });
    },

    saveConfiguration: (page) => {
        Dashboard.showLoadingMsg();
        ApiClient.getPluginConfiguration(hideEmptyFoldersConfigurationPage.pluginUniqueId).then(function (config) {
            config.ScanFolderPath = page.querySelector("#scanFolderPath").value || "";
            config.ScanIntervalMinutes = parseInt(page.querySelector("#scanIntervalMinutes").value) || 60;
            if (config.ScanIntervalMinutes < 1) {
                config.ScanIntervalMinutes = 60;
            }
            config.VideoExtensions = page.querySelector("#videoExtensions").value || "avi,mp4,mkv,mov,wmv,flv,webm,m4v,mpg,mpeg,ts,mts,m2ts,3gp,3g2,f4v";

            ApiClient.updatePluginConfiguration(hideEmptyFoldersConfigurationPage.pluginUniqueId, config).then(function (result) {
                Dashboard.processPluginConfigurationUpdateResult(result);
            });
        });
    },
};

// Wait for the page to load
document.addEventListener("DOMContentLoaded", function () {
    const page = document.querySelector("#hideEmptyFoldersConfigurationPage");

    if (page) {
        // Load configuration when page shows
        page.addEventListener("pageshow", function () {
            hideEmptyFoldersConfigurationPage.loadConfiguration(page);
        });

        // Save configuration on form submit
        const form = page.querySelector("#hideEmptyFoldersConfigurationForm");
        if (form) {
            form.addEventListener("submit", function (e) {
                e.preventDefault();
                hideEmptyFoldersConfigurationPage.saveConfiguration(page);
                return false;
            });
        }
    }
});

// Also try to bind immediately in case DOMContentLoaded already fired
(function () {
    const page = document.querySelector("#hideEmptyFoldersConfigurationPage");

    if (page) {
        // Load configuration when page shows
        page.addEventListener("pageshow", function () {
            hideEmptyFoldersConfigurationPage.loadConfiguration(page);
        });

        // Save configuration on form submit
        const form = page.querySelector("#hideEmptyFoldersConfigurationForm");
        if (form) {
            form.addEventListener("submit", function (e) {
                e.preventDefault();
                hideEmptyFoldersConfigurationPage.saveConfiguration(page);
                return false;
            });
        }
    }
})();
