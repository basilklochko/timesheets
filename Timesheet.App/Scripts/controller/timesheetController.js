app.controller('timesheetController', function ($scope, $routeParams, $location, uiFactory, userFactory, clientFactory, timesheetFactory, calendarFactory, securityFactory, cookieFactory) {
    $scope.modify = function (id) {
        $location.path('timesheet/' + id);
    };

    function init() {
        if ($scope.user == undefined) {
            var segments = $location.url().split("/");

            if (segments.length == 3) {
                var url = $location.url();
                var params = url.split("/")[2].split("?");

                if (params.length == 2) {
                    var id = params[0];
                    var tokenString = params[1].split("=");

                    if (tokenString.length == 2) {
                        var token = tokenString[1];

                        if (id.length > 0 && token.length > 0) {
                            uiFactory.block();

                            userFactory.loginWithToken(token).success(function (data) {
                                if (data.length > 0) {
                                    cookieFactory.set('timesheet.auth', data);

                                    $('#userName').text(unescape(data.split('_')[1]));

                                    mymenu.classList.remove("hidden");

                                    $scope.modify(id);
                                }

                                uiFactory.unblock();
                            });

                        }
                    }
                }
            }
        }

        userFactory.init();

        $scope.user = userFactory.getUser();

        if ($scope.user.type == 'Consultant') {
            uiFactory.block();

            clientFactory.getClientsByConsultant().success(function (data) {
                uiFactory.unblock();

                if (data.length == 0) {
                    $location.path('/');
                }
            });
        }

        securityFactory.auth('timesheetController', $scope.user.type);

        if ($location.path() == "/timesheets") {
            uiFactory.block();

            timesheetFactory.getTimesheets().success(function (data) {
                if (data.length == 0) {
                    noTimesheetsAlert.classList.remove('hidden');
                }
                else {
                    angular.forEach(data, function (value) {
                        var startDate = new Date(value.StartDate).getMonth() + 1 + "/" + new Date(value.StartDate).getDate() + "/" + new Date(value.StartDate).getFullYear();
                        var endtDate = new Date(value.EndDate).getMonth() + 1 + "/" + new Date(value.EndDate).getDate() + "/" + new Date(value.EndDate).getFullYear();

                        value.UpdatedDTS = new Date(value.UpdatedDTS);
                        value.StartDate = startDate;
                        value.EndDate = endtDate;
                    });

                    $scope.timesheets = data;
                }

                uiFactory.unblock();
            });
        }

        if ($location.path() == "/addtimesheet") {
            uiFactory.block();
            calendarFactory.init($scope.user.type);

            $scope.consultantVisibility = true;
            $scope.vendorVisibility = true;
            $scope.clientVisibility = true;

            $scope.timesheet = {
                TimesheetStatus: 'Pending'
            };

            clientFactory.getClientsByConsultant().success(function (data) {
                $scope.clients = data;
                $scope.setClient(data[0].id, data[0].Client.UserName);

                uiFactory.unblock();
            });
        }
        
        if ($location.path() == "/timesheet/" + $scope.id) {
            uiFactory.block();
            calendarFactory.init($scope.user.type);

            timesheetFactory.getTimesheet($scope.id).success(function (data) {
                $scope.timesheet = {
                    TimesheetStatus: data.TimesheetStatus
                };

                $scope.comment = data.Comment;
                $scope.setClient(data.ClientConsultantId, data.Client.UserName);

                calendarFactory.renderDays(new Date(data.StartDate), new Date(data.EndDate));
                calendarFactory.setDays(data.Days);

                if ($scope.user.type == 'Vendor' || $scope.user.type == 'Client') {
                    $('textarea').attr('disabled', 'disabled');
                    $('input').attr('disabled', 'disabled');                    
                }

                uiFactory.unblock();
            });
        }
    }

    if ($routeParams.id != undefined) {
        $scope.id = $routeParams.id;
    }

    init();

    $scope.setClient = function (id, name) {
        $scope.clientConsultantId = id;
        $scope.client = name;

        if ($location.path() == "/addtimesheet" || $location.path() == "/timesheet/" + $scope.id) {
            uiFactory.block();

            calendarFactory.clearAllEvents();

            timesheetFactory.getTimesheets(id).success(function (data) {
                $.each(data, function (index, value) {
                    calendarFactory.renderEvent(new Date(value.StartDate), new Date(value.EndDate));
                });

                uiFactory.unblock();
            });
        }
    }

    $scope.getStatusClass = function (status) {
        return status.toLowerCase();
    };

    $scope.gohome = function () {
        $location.path('timesheets');
    }

    $scope.save = function (status) {
        timesheetAlert.classList.add('hidden');

        if ($scope.clientConsultantId == undefined || $scope.clientConsultantId == null) {
            $scope.timesheetAlertMessage = "Please select client";
            timesheetAlert.classList.remove('hidden');

            return;
        }

        if ($('.timesheetEntry').length == 0) {
            $scope.timesheetAlertMessage = "Please select days";
            timesheetAlert.classList.remove('hidden');

            return;
        }

        var days = [];

        $('.timesheetEntry').each(function () {
            days.push({ day: new Date(this.id.replace('day_', '').replace('_', '/').replace('_', '/')), worked: Number(this.value) });
        });

        var timesheetToSave = {
            startDate: days[0].day,
            endDate: days[days.length - 1].day,
            clientConsultantId: $scope.clientConsultantId,
            days: days,
            comment: $scope.comment,
            status: $scope.timesheet.TimesheetStatus
        };

        uiFactory.block();
        
        if ($scope.id != undefined) {
            timesheetToSave.id = $scope.id;
        }

        timesheetFactory.save(timesheetToSave).success(function (data) {
            uiFactory.unblock();

            if (data > 0) {
                if (status == undefined) {
                    $location.path('timesheets');
                }
                else {
                    $scope.id = data;
                    $scope.saveStatus(status);
                }
            }
            else {
                $scope.timesheetAlertMessage = "Timesheet changes were not saved, please try again";
                timesheetAlert.classList.remove('hidden');
            }
        });
    }

    $scope.confirmdelete = function (id) {
        $location.path('confirmdeletetimesheet/' + id);
    };

    $scope.delete = function (id) {
        timesheetAlert.classList.add('hidden');

        uiFactory.block();

        timesheetFactory.delete(id).success(function (data) {
            uiFactory.unblock();

            if (data > 0) {
                $location.path('timesheets');
            }
            else {
                $scope.timesheetAlertMessage = "Timesheet was not deleted, please try again";
                timesheetAlert.classList.remove('hidden');
            }
        });
    };

    $scope.changeStatus = function (status) {
        if ($scope.id == undefined) {
            $scope.save(status);
        }
        else {
            $scope.saveStatus(status);
        }
    };

    $scope.saveStatus = function (status) {
        timesheetAlert.classList.add('hidden');

        uiFactory.block();

        timesheetFactory.changeStatus($scope.id, status).success(function (data) {
            uiFactory.unblock();

            if (data == "True") {
                $location.path('timesheets');
            }
            else {
                $scope.timesheetAlertMessage = "Timesheet status was not changed, please try again";
                timesheetAlert.classList.remove('hidden');
            }
        });
    };
});