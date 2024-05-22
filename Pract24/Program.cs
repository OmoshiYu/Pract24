using System;
using System.Collections.Generic;
using System.Text;
using static Pract24.Program;

namespace Pract24
{
    internal class Program
    {
        public enum OutputFormat
        {
            Markdown,
            Html
        }

        public interface IListStrategy
        {
            void Start(StringBuilder sb);
            void AddListItem(StringBuilder sb, string item);
            void End(StringBuilder sb);
        }

        public class TextProcessor<LS>
            where LS : IListStrategy, new()
        {
            private StringBuilder _sb = new StringBuilder();
            private IListStrategy _listStrategy = new LS();

            public void AppendList(IEnumerable<string> items)
            {
                _listStrategy.Start(_sb);
                foreach (var item in items)
                    _listStrategy.AddListItem(_sb, item);
                _listStrategy.End(_sb);
            }

            public void Clear()
            {
                _sb.Clear();
            }

            public override string ToString()
            {
                return _sb.ToString();
            }
        }

        public class HtmlListStrategy : IListStrategy
        {
            public void Start(StringBuilder sb) => sb.AppendLine("<ul>");
            public void End(StringBuilder sb) => sb.AppendLine("</ul>");
            public void AddListItem(StringBuilder sb, string item)
            {
                sb.AppendLine($"    <li>{item}</li>");
            }
        }

        public class MarkdownListStrategy : IListStrategy
        {
            public void Start(StringBuilder sb) { }
            public void End(StringBuilder sb) { }
            public void AddListItem(StringBuilder sb, string item)
            {
                sb.AppendLine($"    * {item}");
            }
        }

        public class SimpleJSONStrategy : IListStrategy
        {
            public void Start(StringBuilder sb)
            {
                sb.AppendLine($"List{listNumber++}");
                sb.AppendLine("{");
            }

            public void AddListItem(StringBuilder sb, string item)
            {
                sb.AppendLine($"\titem{itemNumber++} = {item}");
            }

            public void End(StringBuilder sb)
            {
                sb.AppendLine("}");
                itemNumber = 0;
            }

            private uint itemNumber = 0;
            private static uint listNumber = 0;
        }

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            var htmlProcessor = new TextProcessor<HtmlListStrategy>();
            htmlProcessor.Clear();
            htmlProcessor.AppendList(new[] { "Шторм", "Мотивация", "Сила" });
            Console.WriteLine(htmlProcessor);

            Console.ForegroundColor = ConsoleColor.Magenta;
            var jsonProcessor = new TextProcessor<SimpleJSONStrategy>();
            jsonProcessor.Clear();
            jsonProcessor.AppendList(new[] { "Новая игра", "Загрузить последнее сохранение", "Настройки", "Ливнуть (Выход)" });
            jsonProcessor.AppendList(new[] { "Продолжить", "Сохраниться", "Настройки", "Покинуть катку (Выход)" });
            Console.WriteLine(jsonProcessor);

            Console.ForegroundColor = ConsoleColor.Green;
            var markdownProcessor = new TextProcessor<MarkdownListStrategy>();
            markdownProcessor.Clear();
            markdownProcessor.AppendList(new[] { "Проект 'Новая Жизнь'", "[ДАННЫЕ УДАЛЕНЫ]" });
            Console.WriteLine(markdownProcessor);

            Console.ResetColor();
            Console.ReadKey(true);
        }
    }
}
