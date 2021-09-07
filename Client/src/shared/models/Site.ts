import { ContentItem, Elements } from '@kentico/kontent-delivery';

import type { IRoute, Route } from "./Route";

export interface ISite {
  name: string;
  routes: IRoute[];
}

export class Site extends ContentItem {
  static codename = "site";
  name!: Elements.TextElement;
  routes!: Elements.LinkedItemsElement<Route>;

  getModel(): ISite {
    return {
      name: this.name.value,
      routes: this.routes.value.map((route) => route.getModel()),
    };
  }
}
