// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
$(function () {
    if ("tinymce" in window) {
        tinymce.init({
            selector: '.html-editor',
            plugins: 'advlist autolink lists link image code charmap print preview hr anchor pagebreak',
            toolbar_mode: 'floating',
            language: 'ru',
            font_formats:
                "Inter;Montserrat;Raleway;Andale Mono=andale mono,times; Arial=arial,helvetica,sans-serif; Arial Black=arial black,avant garde; Book Antiqua=book antiqua,palatino; Comic Sans MS=comic sans ms,sans-serif; Courier New=courier new,courier; Georgia=georgia,palatino; Helvetica=helvetica; Impact=impact,chicago; Oswald=oswald; Symbol=symbol; Tahoma=tahoma,arial,helvetica,sans-serif; Terminal=terminal,monaco; Times New Roman=times new roman,times; Trebuchet MS=trebuchet ms,geneva; Verdana=verdana,geneva; Webdings=webdings; Wingdings=wingdings,zapf dingbats",
            content_css: '/css/style.min.css',
            content_style: 'body { background-color: #fff }',

            images_upload_url: getImageUploadHandler(),

            images_upload_handler: function (blobInfo, success, failure) {

                const formData = new FormData();

                formData.append("file", blobInfo.blob());

                $.ajax({
                    type: "POST",
                    enctype: 'multipart/form-data',
                    url: getImageUploadHandler(),
                    data: formData,
                    processData: false,
                    contentType: false,
                    cache: false,
                    timeout: 600000,
                    success: function (data) {
                        success(data.urls[0]);
                    },
                    error: function () {
                        failure("ошибка");
                    }
                });
            },
        });
    }

})