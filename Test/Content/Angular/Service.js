app.service("myService", function ($http) {

    this.UserLogin = function (User) {
        var response = $http({
            method: "Post",
            url: "/Login/Login",
            data: JSON.stringify(User),
            dataType: "json"
        });
        return response;
    }

});