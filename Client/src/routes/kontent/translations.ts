import type { SapperRequest, SapperResponse } from "@sapper/server";

import { deliveryClient } from '../../shared/kontent';
import { Translation } from '../../shared/models/Translation';

export function get(
  request: SapperRequest,
  response: SapperResponse,
  next: () => void
) {
  let originalBody = "";

  request.on("data", (chunk) => {
    originalBody += chunk;
  });

  request.on("end", async () => {
    try {
      const kontentResponse = await deliveryClient()
        .items<Translation>()
        .type(Translation.codename)
        .toPromise();

      response.setHeader("Content-Type", "application/json");
      response.end(
        JSON.stringify(
          kontentResponse.items.reduce((translations, translation) => {
            translations[translation.system.codename] =
              translation.content.value;
            return translations;
          }, {})
        )
      );
    } catch (error) {
      switch (error.code) {
        case "ECONNREFUSED":
          response.statusCode = 404;
          break;

        default:
          response.statusCode = 400;
          break;
      }

      response.end(JSON.stringify(error));
    }
  });
}
