$(document).ready(function () {
    $("#DepartmentId").change(function () {
        $("#CityId").empty();
        $("#CityId").append('<option value="0">[Select a city...]</option>');
        $.ajax({
            type: 'POST',
            url: Url,
            dataType: 'json',
            data: { departmentId: $("#DepartmentId").val() },
            success: function (data) {
                $.each(data, function (i, data) {
                    $("#CityId").append('<option value="'
                        + data.CityId + '">'
                        + data.Name + '</option>');
                });
            },
            error: function (ex) {
                alert('Failed to retrieve cities.'+ ex);
            }
        });
        return false;
    })
});
