app.factory('calendarFactory', function () {
    var factory = {};

    var isSelected = false;

    factory.init = function (type) {
        angular.element(document).ready(function () {
            var calendar = $('#calendar').fullCalendar({
                header: {
                    left: 'prev',
                    center: 'title',
                    right: 'next'
                },
                selectable: type == "Consultant" ? true : false,
                selectHelper: true,
                dayRender: function (date, cell) {
                    cell.addTouch();
                },
                select: function (start, end) {
                    var isValid = true;

                    $.each(calendar.fullCalendar('clientEvents'), function (index, value) {
                        if (value.id != 1000) {
                            if (value.end >= start) {
                                isValid = false;
                            }
                        }
                    });

                    calendar.fullCalendar('unselect');

                    if (isValid) {
                        if (isSelected) {
                            $("td .small").remove();
                            
                            calendar.fullCalendar('removeEvents', 1000);
                        }

                        var event = {
                            id: 1000,
                            start: start,
                            end: end
                        };

                        calendar.fullCalendar('renderEvent',
                            event,
                            true
                        );

                        isSelected = true;

                        factory.renderDays(start, end);
                    }
                },
                viewDisplay: function(view) {
                    factory.setTotal();
                }
            });
        });
    };

    factory.clearAllEvents = function () {
        $('#calendar').fullCalendar('removeEvents');
    };

    factory.renderEvent = function (start, end) {
        var event = {
            start: start,
            end: end
        };

        if ($('.timesheetEntry').length > 0) {
            var currentStart = new Date($('.timesheetEntry').first().attr('ng-model').replace('day_', ''));
            var currentEnd = new Date($('.timesheetEntry').last().attr('ng-model').replace('day_', ''));

            if ($('label[for="day_' + $.fullCalendar.formatDate(event.start, 'MM_dd_yyyy') + '"]').length == 0) {
                event.backgroundColor = 'black';
            }
            else {
                event.id = 1000;

                isSelected = true;
            }
        }
        else {
            event.backgroundColor = 'black';
        }

        $('#calendar').fullCalendar('renderEvent', event, true);
    };

    factory.setTotal = function () {
        var total = 0;

        $('.timesheetEntry').each(function () {
            total += Number(this.value);

            var date = $.fullCalendar.formatDate(new Date($(this).attr('id').replace('day_', '').replace('_', '/').replace('_', '/')), 'yyyy-MM-dd')

            $("td[data-date='" + date + "'] div .small").remove();
            $("td[data-date='" + date + "'] div").first().prepend("<span class='col-xs-1 small' style='color: blue'>" + Number(this.value) + "hrs</span>");
        });

        $('#total').text(total + " hours");
    };

    factory.renderDays = function (start, end) {
        $('#dates').empty();
        $('#totalLabel').removeClass("hidden");

        var days = factory.dateRange(start, end);

        $('#dates').append('<form class="form-horizontal" id="daysForm" name="daysForm" role="form"></form>');

        $.each(days, function (index, dt) {
            var html = '<div class="form-group">';
            html += '<label for="day_' + dt.replace('/', '_').replace('/', '_') + '" class="col-sm-6 control-label">' + $.fullCalendar.formatDate(new Date(dt), 'ddd MM/dd/yyyy') + '</label>';
            html += '<div class="col-sm-3">';
            html += '<input class="form-control timesheetEntry" type="number" id="day_' + dt.replace('/', '_').replace('/', '_') + '" ng-model="day_' + dt.replace('/', '_').replace('/', '_') + '" value="8">';
            html += '</div><label class="col-sm-1 control-label">hours</label>';
            html += '</div>';

            $('#daysForm').append(html);            
        });

        $('.timesheetEntry').bind('keyup', function () {
            factory.setTotal();
        });

        $('.timesheetEntry').bind('change', function () {
            factory.setTotal();
        });

        factory.setTotal();
    };

    factory.setDays = function (days) {
        $.each(days, function (index, value) {
            if (index == 0) {
                $('#calendar').fullCalendar('gotoDate', new Date(value.Day).getFullYear(), new Date(value.Day).getMonth(), new Date(value.Day).getDay());
            }

            $('#day_' + $.fullCalendar.formatDate(new Date(value.Day), 'MM_dd_yyyy')).val(value.Worked).change();
        });
    };

    factory.dateRange = function (from, to) {
        var DA = [$.fullCalendar.formatDate(from, 'MM/dd/yyyy')];
        var m = from.getMonth();
        var incr = from.getDate();

        while (from < to) {
            from.setDate(++incr);
            DA.push($.fullCalendar.formatDate(from, 'MM/dd/yyyy'));

            if (from.getMonth() != m) {
                m = from.getMonth();
                incr = 1;
            }
        }

        return DA;
    };

    return factory;
});