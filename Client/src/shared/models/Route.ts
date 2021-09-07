import { ContentItem, Elements } from '@kentico/kontent-delivery';

import type { IIcon, Icon } from "./Icon";

export interface IRoute {
  name: string;
  icon: IIcon;
  options: "naked"[];
  route: string;
  routes: IRoute[];
}

export class Route extends ContentItem {
  static codename = "route";
  name!: Elements.TextElement;
  icon!: Elements.LinkedItemsElement<Icon>;
  options!: Elements.MultipleChoiceElement;
  route!: Elements.UrlSlugElement;
  routes!: Elements.LinkedItemsElement<Route>;

  getModel(): IRoute {
    return {
      name: this.name.value,
      icon: this.icon.value[0]?.getModel(),
      options: this.options.value.map((option) => option.codename) as any[],
      route: this.route.value,
      routes: this.routes.value.map((route) => route.getModel()),
    };
  }
}
