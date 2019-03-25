var marksViewMode = {
    students: [],
    courses: [],
    serviceEndpoint: "https://localhost:44356/api"
};

$(document).ready(function () {
    Init();
});

function Init() {
    $.validate({
        lang: 'en',
        form: '#AddCourseForm',
        onSuccess: function (courseForm) {

            var course = {
                id: GetGuid(),
                code: $(courseForm).find('#code').val(),
                name: $(courseForm).find('#name').val(),
                description: $(courseForm).find('#description').val()
            };

            $.ajax({
                url: marksViewMode.serviceEndpoint +"/course",
                type: "POST",
                contentType: "application/json",
                dataType: 'json',
                data: JSON.stringify(course),
                success: function () {

                    marksViewMode.courses.push(course);
                    $("#marksTable thead tr").append('<th id="' + course.code + '">' + course.name + '</th>');

                    var trs = $("#marksTable tbody tr");

                    for (var i = 0; i < trs.length; i++) {
                        $(trs[i]).append('<td onclick="SetGrade(this, \'' + trs[i].id + '\');"></td>');
                    }

                    CloseDialog();
                },
                error: function (ex) {
                    alert(ex.responseText);
                }
            });

            return false; // Will stop the submission of the form
        }
    });

    $.validate({
        lang: 'en',
        form: '#AddStudentForm',
        onSuccess: function (studentForm) {

            var student = {
                id: GetGuid(),
                firstName: $(studentForm).find('#firstName').val(),
                lastName: $(studentForm).find('#lastName').val(),
                address: $(studentForm).find('#address').val(),
                city: $(studentForm).find('#city').val(),
                state: $(studentForm).find('#state').val(),
                dateOfBirth: $(studentForm).find('#bday').val(),
                gender: $(studentForm).find("input[name='gender']:checked").val()
            };

            $.ajax({
                url: marksViewMode.serviceEndpoint + "/student",
                type: "POST",
                contentType: "application/json",
                dataType: 'json',
                data: JSON.stringify(student),
                success: function (result) {

                    marksViewMode.students.push(student);

                    var trId = student.id;
                    $("#marksTable tbody").append('<tr id="' + trId + '"><td >' + student.firstName + ' ' + student.lastName + '</td></tr>');

                    for (var i = 1; i < $("#marksTable thead tr th").length; i++) {
                        $("#" + trId).append('<td onclick="SetGrade(this, \'' + student.id + '\');"></td>');
                    }

                    CloseDialog();
                },
                error: function (ex) {
                    alert(ex.responseText);
                }
            });

            return false; // Will stop the submission of the form
        }
    });

    InitCourses();
}

function InitStudents() {
    var filter = $("#student-filter").val();

    var url = "";
    if (filter === "") {
        url = marksViewMode.serviceEndpoint + "/student/passedCourses";
    } else {
        url = marksViewMode.serviceEndpoint + "/student/passedCourses?filterByFullName=" + filter;
    }

    $.ajax({
        url: url,
        type: "GET",
        contentType: "application/json",
        dataType: 'json',
        success: function (result) {

            for (var i = 0; i < result.length; i++) {

                var student = result[i];

                marksViewMode.students.push(student);

                var trId = student.id;
                $("#marksTable tbody").append('<tr id="' + trId + '"><td >' + student.fullName + '</td></tr>');

                for (var j = 1; j < $("#marksTable thead tr th").length; j++) {

                    var courseCode = parseInt($("#marksTable thead tr th")[j].id);
                    var grade = "";

                    if (student.courses.length !== 0) {

                        var course = student.courses.filter(c => c.code === courseCode)[0];
                        if (course !== undefined) {
                            var grade = course.grade;
                        }
                    }

                    $("#" + trId).append('<td onclick="SetGrade(this, \'' + student.id + '\');">' + grade + '</td>');
                }
            }
        },
        error: function (ex) {
            alert(ex.responseText);
        }
    });
}

function InitCourses() {

    var filter = $("#course-filter").val();
    var url = "";
    if (filter === "") {
        url = marksViewMode.serviceEndpoint + "/course";
    } else {
        url = marksViewMode.serviceEndpoint + "/course?filterByName=" + filter;
    }

    $.ajax({
        url: url,
        type: "GET",
        contentType: "application/json",
        dataType: 'json',
        success: function (result) {

            for (var i = 0; i < result.length; i++) {

                marksViewMode.courses.push(result[i]);
                $("#marksTable thead tr").append('<th id="' + result[i].code + '">' + result[i].name + '</th>');
            }

            InitStudents();
        },
        error: function (ex) {
            alert(ex.responseText);
        }
    });
}

function GetGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}

function CloseDialog() {

    $('#AddCourseForm').get(0).reset();
    $('#AddStudentForm').get(0).reset();
    $(".dialog-model").css("visibility", "hidden");
}

function ShowDialog(id) {
    $("#" + id).height($("body").height());
    $("#" + id).width($("body").width());
    $("#" + id).css("visibility", "visible");
}

var currentTdSelected;
var oldTdValue;
function SetGrade(td, studentId) {

    if (currentTdSelected !== td) {
        $(currentTdSelected).empty();
        $(currentTdSelected).html(oldTdValue);
    }

    currentTdSelected = td;

    if ($(td).find('select').length === 0) {

        oldTdValue = $(td).html();
        $(td).empty();

        var tdIndex = $(td).index();
        var courseCode = $("#marksTable thead tr th")[tdIndex].id;

        $(td).append(`<select onchange="GradeSelected(this,'` + studentId + `', '` + courseCode + `')">
                        <option value=""></option>
                        <option value="6">6</option>
                        <option value="7">7</option>
                        <option value="8">8</option>
                        <option value="9">9</option>
                        <option value="10">10</option>
                     </select>`);
    }
}

function GradeSelected(selectElement, studentId, courseCode) {

    if ($(selectElement).val() !== "") {
        $.ajax({
            url: marksViewMode.serviceEndpoint + "/student/setCrade",
            type: "POST",
            contentType: "application/json",
            dataType: 'json',
            data: JSON.stringify({ Id: studentId, CourseCode: courseCode, Grade: $(selectElement).val() }),
            success: function (result) {
                var td = $(selectElement).parents('td');
                td.html($(selectElement).val());

                $(selectElement).remove();
            },
            error: function (ex) {
                alert(ex.responseText);
            }
        });
    } else {
        var td = $(selectElement).parents('td');
        td.html($(selectElement).val());

        $(selectElement).remove();
    }
}

function Filter(input, type) {

    if (type === 'Student') {
        $("#marksTable tbody").empty();
        InitStudents();
    }
    else {
        $("#marksTable tbody").empty();
        $("#marksTable thead tr").empty();
        $("#marksTable thead tr").append('<th style="background: black; color: yellow;">INSSIO</th>');
        InitCourses();
    }
}