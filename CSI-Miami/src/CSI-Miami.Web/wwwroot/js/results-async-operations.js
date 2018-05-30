
$(function () {
    $(".table").on('submit', '.edit-movie', function (event) {
        event.preventDefault();

        var form = $(this);
        var id = form.children('#movie_Id').val();

        var newMovieName = $('#movie-title-' + id).val();
        var newDirectorName = $("#movie-director-" + id).val();
        var newReleaseDate = $("#movie-releasedate-" + id).val();

        var currentMovieName = $('#table-movie-title-' + id);
        var currentMovieDirectorName = $('#table-movie-directorName-' + id);
        var currentMovieReleaseDate = $('#table-movie-releaseDate-' + id);
        console.log(currentMovieReleaseDate.html());


        var url = this.action;

        var data = form.serialize();

        $(`#myModalNorm-${id}`).modal('hide');

        $.ajax({
            type: 'POST',
            url: url,
            data: data,
            success: function (response, status, headers) {
                if (response.value.isEdited === true) {
                    currentMovieName.html(newMovieName);
                    currentMovieDirectorName.html(newDirectorName);
                    currentMovieReleaseDate.html(newReleaseDate);
                    window.alert('Movie was successfully edited!');
                }
                else {
                    window.alert("The edit failed. Please try again later!");
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr);
                alert(xhr.status);
                alert(thrownError);
            }
        });

    })
})