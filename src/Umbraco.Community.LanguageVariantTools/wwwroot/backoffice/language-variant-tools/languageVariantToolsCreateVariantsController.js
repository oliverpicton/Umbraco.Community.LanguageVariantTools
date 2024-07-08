angular.module("umbraco").controller("languageVariantToolsCreateVariantsController", function ($scope, $http, navigationService, $location) {
    $scope.busy = false;
    $scope.success = false;
    $scope.error = false;
    $scope.errorMessage = "";
    $scope.includeChildren = false;

    $scope.createVariants = async function (nodeId) {
        $scope.busy = true;

        var culture = $location.search().cculture;

        if (!culture) {
            culture = $location.search().mculture;
        }
        
        var result = await $http({
            method: "GET",
            params: {
                nodeId: nodeId,
                includeChildren: $scope.includeChildren,
                culture: culture
            },
            url: "backoffice/LanguageVariantTools/LanguageVariant/Create/"
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
        location.reload();
    };
    $scope.includeChildrenClick = function () {
        $scope.includeChildren = !$scope.includeChildren;
    };
});