export const environment = {
  production: true,
  api: {
    url: "{{API_URL}}"
  },
  microsoft: {
    clientId: "{{MICROSOFT_CLIENT_ID}}",
    redirectUri: "https://localhost:4200/callback/microsoft",
    authority: "https://login.microsoftonline.com/common/",
    scopes: ["{{MICROSOFT_SCOPES}}"]
  }
};
