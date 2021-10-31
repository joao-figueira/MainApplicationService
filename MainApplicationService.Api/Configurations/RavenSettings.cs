namespace MainApplicationService.Api.Configurations
{
    public class RavenSettings
    {
        public string[]? Urls { get; set; }
        public string? DatabaseName { get; set; }
        public string? CertificatePath { get; set; }
        public string? CertificatePassword { get; set; }
    }
}
