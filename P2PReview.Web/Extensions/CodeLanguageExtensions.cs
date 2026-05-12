using P2PReview.Domain.Enums;

namespace P2PReview.Web.Extensions
{
    public static class CodeLanguageExtensions
    {
        public static string ToDisplayName(CodeLanguage language)
        {
            return language switch
            {
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
    }
}
