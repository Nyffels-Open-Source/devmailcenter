using System.Text.Json.Serialization;

namespace DevMailCenter.External.Models;

public class MicrosoftApiMail
{
    [JsonPropertyName("message")] public MicrosoftApiMailMessage Message { get; set; }

    [JsonPropertyName("saveToSentItems")] public bool SaveToSentItems { get; set; }
}

public class MicrosoftApiMailMessage
{
    [JsonPropertyName("subject")] public string Subject { get; set; }

    [JsonPropertyName("body")] public MicrosoftApiMailMessageBody Body { get; set; }

    [JsonPropertyName("toRecipients")] public List<MicrosoftApiMailMessageRecipient> ToRecipients { get; set; }

    [JsonPropertyName("ccRecipients")] public List<MicrosoftApiMailMessageRecipient> CcRecipients { get; set; }

    [JsonPropertyName("bccRecipients")] public List<MicrosoftApiMailMessageRecipient> BccRecipients { get; set; }

    [JsonPropertyName("hasAttachments")] public bool HasAttachments { get; set; }

    [JsonPropertyName("importance")] public string Importance { get; set; } // high, low, normal

    [JsonPropertyName("inferenceClassification")] public string InterferenceClassification { get; set; }

    [JsonPropertyName("attachments")] public List<MicrosoftApiMailMessageAttachment> Attachments { get; set; }
}

public class MicrosoftApiMailMessageBody
{
    [JsonPropertyName("contentType")] public string Type { get; set; }
    [JsonPropertyName("content")] public string Content { get; set; }
}

public class MicrosoftApiMailMessageRecipient
{
    [JsonPropertyName("emailAddress")] public MicrosoftApiMailMessageRecipientEmailAddress EmailAddress { get; set; }
}

public class MicrosoftApiMailMessageRecipientEmailAddress
{
    [JsonPropertyName("address")] public string Address { get; set; }
}

public class MicrosoftApiMailMessageAttachment
{
    [JsonPropertyName("@odata.type")] public string Type { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("contentType")] public string ContentType { get; set; }
    [JsonPropertyName("contentBytes")] public byte[] ContentBytes { get; set; }
}