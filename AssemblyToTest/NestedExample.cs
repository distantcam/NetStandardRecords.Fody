public class NestedExample
{
    public record NestedRecord(int Number, string String)
    {
        public record NestedNestedRecord(int Number, string String);
    }
}
