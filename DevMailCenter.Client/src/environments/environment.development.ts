export const environment = {
  production: false,
  api: {
    url: "http://localhost:5125"
  },
  microsoft: {
    clientId: "{{MICROSOFT_CLIENT_ID}}",
    redirectUri: "https://localhost:4200/callback/microsoft",
    authority: "https://login.microsoftonline.com/common/",
    scopes: []
  }
};
