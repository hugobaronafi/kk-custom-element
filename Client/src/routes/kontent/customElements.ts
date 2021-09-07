import type { SapperRequest, SapperResponse } from "@sapper/server";

import { deliveryClient, extractComponents } from '../../shared/kontent';
import { Code as CodeModel } from '../../shared/models/Code';
import { CustomElement } from '../../shared/models/CustomElement';

import type { ICode } from "../../shared/models/Code";
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
    const components = new Map<string, ICode>();
    const richTextResolver = extractComponents(
      components,
      new Map([[CodeModel.codename, (item) => (item as CodeModel).getModel()]])
    );

    try {
      const customElements = await deliveryClient()
        .items<CustomElement>()
        .type(CustomElement.codename)
        .queryConfig({
          richTextResolver,
        })
        .toPromise();

      response.setHeader("Content-Type", "application/json");
      response.end(
        JSON.stringify({
          customElements: customElements.items.map((element) =>
            element.getModel()
          ),
          components,
        })
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
