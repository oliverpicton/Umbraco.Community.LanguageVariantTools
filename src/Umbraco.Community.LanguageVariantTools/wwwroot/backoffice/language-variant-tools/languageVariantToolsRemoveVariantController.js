angular.module("umbraco").controller("languageVariantToolsRemoveVariantController", function ($scope, $http, navigationService, $location, $route) {
    $scope.busy = false;
    $scope.success = false;
    $scope.error = false;
    $scope.errorMessage = "";

    $scope.removeVariant = async function (nodeId) {
        $scope.busy = true;

        var culture = $location.search().cculture;

        if (!culture) {
            culture = $location.search().mculture;
        }

        var result =  await $http({
            method: "GET",
            params: {
                nodeId: $scope.currentNode.id,
                culture: culture
            },
            url: "backoffice/LanguageVariantTools/LanguageVariant/Remove/"
        });

        if (result.data.IsSuccess) {
            $scope.success = true;
        }
        else {
            $scope.errorMessage = $scope.data.Error.Message;
            $scope.error = true;
        }

        $scope.busy = false;
    }
    $scope.closeDialog = function () {
        navigationService.hideDialog();
        $route.reload();
    };
});