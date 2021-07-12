namespace ParlanceBackend.Models
{
    public class TranslationDelta
    {
        public string Context { get; set; }
        public string Key { get; set; }
        public string[] Translations { get; set; }
        public bool Unfinished { get; set; }
    }
}