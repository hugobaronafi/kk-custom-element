import type { SapperRequest, SapperResponse } from "@sapper/server";

import { deliveryClient } from '../../shared/kontent';
import { Site } from '../../shared/models/Site';

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
        .item<Site>(Site.codename)
        .depthParameter(6)
        .toPromise();

      response.setHeader("Content-Type", "application/json");
      response.end(JSON.stringify(kontentResponse.item.getModel()));
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
