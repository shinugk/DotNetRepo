import { AuthConfig } from 'angular-oauth2-oidc';

export const authConfig: AuthConfig = {
  issuer: 'https://accounts.google.com',
  clientId: '250228091409-kugdd045lkeh9i8uvrkuhaa28vc7jr4r.apps.googleusercontent.com',//'GOOGLE_CLIENT_ID' from https://console.cloud.google.com/auth/clients/250228091409-kugdd045lkeh9i8uvrkuhaa28vc7jr4r.apps.googleusercontent.com?project=itracker-468520
  redirectUri: window.location.origin,
  scope: 'openid profile email',
  strictDiscoveryDocumentValidation: false,
};
