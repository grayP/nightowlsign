
    $(document).ready(function () {
        $("[data-pdsa-action]").on("click", function (e) {
            e.preventDefault();
            $("#EventCommand").val($(this).data("pdsa-action"));
            $("#EventArgument").val($(this).data("pdsa-val"));
            if ($(this).data("pdsa-action") == "delete") {
                if (confirm("Delete this record?")) {
                    $("form").submit();
                }
            }
            else {
                $("form").submit();
            }
        });



    });


    $(function () {
        $(window).on("load resize", function () {
            $(".fill-screen").css("height", window.innerHeight);
        });

    });




