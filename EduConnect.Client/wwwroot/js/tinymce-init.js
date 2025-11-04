window.tinyHelper = {
    initTiny: function (id) {
        // Đảm bảo TinyMCE được khởi tạo lại đúng DOM phần tử
        setTimeout(() => {
            if (tinymce.get(id)) {
                tinymce.get(id).remove();
            }

            tinymce.init({
                selector: `#${id}`,
                height: 350,
                menubar: false,
                plugins: 'image link lists code table',
                toolbar: 'undo redo | bold italic underline | bullist numlist | link image | code',
                content_style: 'body { font-family: Helvetica,Arial,sans-serif; font-size:14px }',
                setup: (editor) => {
                    editor.on('change', () => editor.save());
                }
            });
        }, 200);
    },

    destroyTiny: function (id) {
        const ed = tinymce.get(id);
        if (ed) ed.remove();
    },

    getContent: function (id) {
        const ed = tinymce.get(id);
        return ed ? ed.getContent() : '';
    }
};
