$(function () {
    $(".table").on('submit', '.edit-test', function (event) {
        event.preventDefault();

        var form = $(this);

        var id = form.children('#Id').val();

        var url = this.action + '/' + id;
        console.log(url);

        //var movieName = $('#movie-title-' + id).val();
        //var directorName = $("#movie-director-" + id).val();
        //var releaseDate = $("movie-releasedate-" + id).val();

        var data = form.serialize();


        $.ajax({
            type: 'POST',
            url: url,
            data: data,
            success: function (response) {
                console.log('fuck yeah');
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr);
                alert(xhr.status);
                alert(thrownError);
            }
        });


    })
})