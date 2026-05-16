using P2PReview.Domain.Enums;

namespace P2PReview.Application.Extensions
{
    public static class CodeLanguageExtensions
    {
        public static string ToDisplayName(CodeLanguage language)
        {
            return language switch
            {
                CodeLanguage.PlainText => "PlainText",
                CodeLanguage.CSharp => "C#",
                CodeLanguage.JavaScript => "JavaScript",
                CodeLanguage.TypeScript => "TypeScript",
                CodeLanguage.Python => "Python",
                CodeLanguage.Java => "Java",
                CodeLanguage.Go => "Go",
                CodeLanguage.Rust => "Rust",
                CodeLanguage.Cpp => "C++",
                CodeLanguage.Php => "PHP",
                CodeLanguage.Swift => "Swift",
                CodeLanguage.Kotlin => "Kotlin",
                CodeLanguage.Sql => "SQL",
                CodeLanguage.Html => "HTML",
                CodeLanguage.Css => "CSS",
                CodeLanguage.Json => "JSON",
                _ => language.ToString()
            };
        }

        public static CodeLanguage FromFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return CodeLanguage.PlainText;

            var ext = Path.GetExtension(fileName).ToLowerInvariant();

            return ext switch
            {
                ".cs" => CodeLanguage.CSharp,
                ".js" => CodeLanguage.JavaScript,
                ".ts" => CodeLanguage.TypeScript,
                ".py" => CodeLanguage.Python,
                ".java" => CodeLanguage.Java,
                ".go" => CodeLanguage.Go,
                ".rs" => CodeLanguage.Rust,
                ".cpp" or ".cc" or ".cxx" or ".hpp" or ".h" => CodeLanguage.Cpp,
                ".php" => CodeLanguage.Php,
                ".swift" => CodeLanguage.Swift,
                ".kt" or ".kts" => CodeLanguage.Kotlin,
                ".sql" => CodeLanguage.Sql,
                ".html" or ".htm" => CodeLanguage.Html,
                ".css" => CodeLanguage.Css,
                ".json" => CodeLanguage.Json,

                _ => CodeLanguage.PlainText
            };
        }

        public static string GetExtension(this CodeLanguage language)
        {
            return language switch
            {
                CodeLanguage.CSharp => ".cs",
                CodeLanguage.JavaScript => ".js",
                CodeLanguage.TypeScript => ".ts",
                CodeLanguage.Python => ".py",
                CodeLanguage.Java => ".java",
                CodeLanguage.Go => ".go",
                CodeLanguage.Rust => ".rs",
                CodeLanguage.Cpp => ".cpp",
                CodeLanguage.Php => ".php",
                CodeLanguage.Swift => ".swift",
                CodeLanguage.Kotlin => ".kt",
                CodeLanguage.Sql => ".sql",
                CodeLanguage.Html => ".html",
                CodeLanguage.Css => ".css",
                CodeLanguage.Json => ".json",

                _ => string.Empty
            };
        }
    }
}
