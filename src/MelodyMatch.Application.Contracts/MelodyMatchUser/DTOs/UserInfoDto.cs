using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MelodyMatch.MelodyMatchUser.DTOs;

public class UserInfoDto
{
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("fullname")]
    public string FullName { get; set; } = string.Empty;
    
    [JsonPropertyName("roles")]
    public List<string> Roles { get; set; } = new ();
    
    [JsonPropertyName("roleNames")]
    public List<string> RoleNames { get; set; } = new();

    [JsonPropertyName("id")]
    public Guid Id { get; set; } = Guid.Empty;
    
    [JsonPropertyName("identityUserId")]
    public Guid IdentityUserId { get; set; } = Guid.Empty;
}