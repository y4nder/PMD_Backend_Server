namespace PMD_Backend.models
{
    public class SecurityKey
    {
        public int Id { get; set; }
        public byte[] Key { get; set; } = null!;
        public byte[] Iv { get; set; } = null!;

        public string? parsedKey { get; set; }
        public string? parsedIv { get; set; }

        public override string ToString()
        {
            return $"ID: {Id}\n" +
                    $"Key : {Convert.ToBase64String(Key)}\n" +
                    $"IV: {Convert.ToBase64String(Iv)}\n";
        }

    }
}
