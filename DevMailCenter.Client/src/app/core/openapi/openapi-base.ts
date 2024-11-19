export class OpenapiBase {
  protected constructor() {
  }

  protected transformOptions(options: any): Promise<any> {
    // if (SwaggerBase.enableBasicAuth) {
    //   const encodedCredentials = btoa(`${SwaggerBase.basicAuthLogin}:${SwaggerBase.basicAuthPassword}`);
    //   options.headers = options.headers.append('authorization', `Basic ${encodedCredentials}`);
    // }

    return Promise.resolve(options);
  }
}
