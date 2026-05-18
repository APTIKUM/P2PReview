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
        minHeight,
        maxHeight,
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
            minimap: {
                enabled: true
            },
            readOnly: readOnly
        });

        window.monacoInterop.editors[id] = editor;

        const updateHeight = () => {

            const contentHeight = editor.getContentHeight();

            const min = parseInt(minHeight);
            const max = parseInt(maxHeight);

            const newHeight =
                Math.min(Math.max(contentHeight, min), max);

            container.style.height = `${newHeight}px`;

            editor.layout();
        };

        updateHeight();

        setTimeout(() => {
            editor.layout();
            updateHeight();
        }, 100);

        editor.onDidContentSizeChange(() => {
            updateHeight();
        });

        editor.onDidChangeModelContent(async () => {

            const value = editor.getValue();

            await dotnetRef.invokeMethodAsync(
                "OnEditorValueChanged",
                value
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
    }
};