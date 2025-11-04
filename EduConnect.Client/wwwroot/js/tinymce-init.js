window.tinyHelper = {
    initTiny: function (id, html) {
        if (tinymce.get(id)) tinymce.remove(`#${id}`);

        setTimeout(() => {
            tinymce.init({
                selector: `#${id}`,
                menubar: 'file edit view insert format tools table help',
                branding: false,
                promotion: false,
                height: 300, // khởi tạo nhỏ, sẽ tự mở rộng
                autoresize_bottom_margin: 20,
                license_key: 'gpl',

                plugins: [
                    'autoresize', 'anchor', 'autolink', 'charmap', 'codesample',
                    'emoticons', 'image', 'link', 'lists', 'media',
                    'table', 'visualblocks', 'wordcount', 'searchreplace',
                    'insertdatetime', 'help', 'mathjax'
                ],

                toolbar:
                    'undo redo | styles | bold italic underline forecolor backcolor | ' +
                    'alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | ' +
                    'link image media table codesample | mathjax formulaBtn | removeformat',

                mathjax: {
                    lib: 'https://cdn.jsdelivr.net/npm/mathjax@3/es5/tex-mml-chtml.js',
                    symbols: { start: '\\(', end: '\\)' }
                },

                setup: (editor) => {
                    editor.ui.registry.addButton('formulaBtn', {
                        text: '𝛴 Chèn công thức',
                        tooltip: 'Thêm công thức toán học',
                        onAction: function () {
                            editor.windowManager.open({
                                title: 'Chèn công thức toán học',
                                body: {
                                    type: 'panel',
                                    items: [
                                        {
                                            type: 'textarea',
                                            name: 'latex',
                                            label: 'Công thức LaTeX',
                                            multiline: true,
                                            minHeight: 100
                                        },
                                        {
                                            type: 'htmlpanel',
                                            html: '<div id="mathPreview" style="border-top:1px solid #ddd;margin-top:10px;padding-top:8px;font-family:Courier New;"></div>'
                                        }
                                    ]
                                },
                                buttons: [
                                    { type: 'cancel', text: 'Hủy' },
                                    { type: 'submit', text: 'Chèn', primary: true }
                                ],
                                onChange: function (api, details) {
                                    if (details.name === 'latex') {
                                        const data = api.getData();
                                        const latex = data.latex.trim();
                                        const preview = document.getElementById('mathPreview');
                                        if (preview) {
                                            preview.innerHTML = latex ? `\\(${latex}\\)` : '';
                                            if (window.MathJax) MathJax.typesetPromise([preview]);
                                        }
                                    }
                                },
                                onSubmit: function (api) {
                                    const data = api.getData();
                                    const latex = data.latex.trim();
                                    if (latex) {
                                        const mathHtml = `<span class="math-tex">\\(${latex}\\)</span>`;
                                        editor.insertContent(mathHtml);
                                    }
                                    api.close();
                                }
                            });
                        }
                    });

                    editor.on('init', function () {
                        console.log('✅ TinyMCE ready on', id);
                        if (html) editor.setContent(html);
                    });
                },

                content_style: `
                    body { font-family: Arial, sans-serif; font-size:14px; line-height:1.6; }
                    img { max-width:100%; height:auto; }
                    .math-tex { background-color:#f8f9fa; padding:2px 4px; border-radius:4px; }
                `,

                // Giới hạn tự mở rộng
                autoresize_max_height: 400,
                min_height: 200
            });
        }, 300);
    },

    getContent: function (id) {
        const ed = tinymce.get(id);
        return ed ? ed.getContent() : '';
    },

    setContent: function (id, html) {
        const ed = tinymce.get(id);
        if (!ed) {
            console.warn('Tiny chưa khởi tạo, thử lại sau...');
            setTimeout(() => window.tinyHelper.setContent(id, html), 500);
            return;
        }
        try {
            ed.setContent(html || '');
        } catch (err) {
            console.warn('TinyMCE setContent error:', err);
        }
    },

    destroyTiny: function (id) {
        const ed = tinymce.get(id);
        if (ed) tinymce.remove(ed);
    }
};
