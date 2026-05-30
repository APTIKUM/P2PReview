window.monacoInterop = {

    editors: {},
    isLoaded: false,

    loadMonaco: async function () {

        if (window.monacoInterop.isLoaded)
            return;

        await new Promise((resolve) => {

            const existing =
                document.querySelector('script[data-monaco-loader]');

            if (existing) {
                existing.onload = resolve;
                return;
            }

            const script = document.createElement("script");

            script.src =
                "https://cdnjs.cloudflare.com/ajax/libs/monaco-editor/0.45.0/min/vs/loader.min.js";

            script.dataset.monacoLoader = "true";

            script.onload = resolve;

            document.body.appendChild(script);
        });

        require.config({
            paths: {
                vs: "https://cdnjs.cloudflare.com/ajax/libs/monaco-editor/0.45.0/min/vs"
            }
        });

        await new Promise((resolve) => {

            require(["vs/editor/editor.main"], function () {

                window.monacoInterop.isLoaded = true;

                resolve();
            });
        });
    },

    init: async function (
        id,
        language,
        value,
        readOnly,
        dotnetRef
    ) {

        await window.monacoInterop.loadMonaco();

        const container = document.getElementById(id);

        if (!container)
            return;

        if (window.monacoInterop.editors[id]) {

            window.monacoInterop.editors[id].dispose();

            delete window.monacoInterop.editors[id];
        }

        const editor = monaco.editor.create(container, {
            value: value || "",
            language: language || "javascript",
            theme: "vs-dark",

            automaticLayout: true,
            scrollBeyondLastLine: false,
            smoothScrolling: true,

            fontSize: 14,
            lineHeight: 20,
            fontLigatures: true,

            minimap: { enabled: false },
            folding: false,
            glyphMargin: false,

            cursorBlinking: "smooth",
            cursorSmoothCaretAnimation: "off",
            cursorStyle: "line",

            wordWrap: "off",
            tabSize: 4,

            renderLineHighlight: "line",
            lineNumbers: "on",

            scrollbar: {
                vertical: "auto",
                horizontal: "auto"
            },

            padding: {
                top: 8,
                bottom: 8
            },

            readOnly: readOnly
        });

        window.monacoInterop.editors[id] = editor;


        setTimeout(() => {
            editor.layout();
        }, 100);

        editor.onDidChangeModelContent(async () => {

            const value = editor.getValue();

            await dotnetRef.invokeMethodAsync(
                "OnEditorValueChanged",
                value
            );
        });


        editor.onMouseDown(function (e) {
            if (!e.target || !e.target.position)
                return;

            const lineNumber = e.target.position.lineNumber;

            dotnetRef.invokeMethodAsync(
                "OnLineClicked",
                lineNumber
            );
        });
    },

    setLanguage: function (id, language) {

        const editor = window.monacoInterop.editors[id];

        if (!editor)
            return;

        const model = editor.getModel();

        monaco.editor.setModelLanguage(model, language);
    },

    setValue: function (id, value) {

        const editor = window.monacoInterop.editors[id];

        if (!editor)
            return;

        if (editor.getValue() !== value) {
            editor.setValue(value ?? "");
        }
    },

    dispose: function (id) {

        const editor = window.monacoInterop.editors[id];

        if (editor) {

            editor.dispose();

            delete window.monacoInterop.editors[id];
        }
    },

    markComments: function (id, comments) {

        const editor = window.monacoInterop.editors[id];

        if (!editor)
            return;

        const decorations = (comments || []).map(c => ({
            range: new monaco.Range(c.line, 1, c.line, 1),
            options: {
                isWholeLine: true,
                className: "comment-line"
            }
        }));

        if (!editor._commentDecorations)
            editor._commentDecorations = [];

        editor._commentDecorations = editor.deltaDecorations(
            editor._commentDecorations,
            decorations
        );
    },

    highlightLine: function (id, line) {

        const editor = window.monacoInterop.editors[id];

        if (!editor)
            return;

        if (!line) {

            editor._activeLineDecoration = editor.deltaDecorations(
                editor._activeLineDecoration || [],
                []
            );

            return;
        }

        const decoration = [{
            range: new monaco.Range(line, 1, line, 1),
            options: {
                isWholeLine: true,
                className: "active-line"
            }
        }];

        editor._activeLineDecoration = editor.deltaDecorations(
            editor._activeLineDecoration || [],
            decoration
        );
    }
};