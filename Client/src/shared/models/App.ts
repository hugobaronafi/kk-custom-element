import { ContentItem, Elements } from '@kentico/kontent-delivery';

import type { ITag, Tag } from "./Tag";

export interface IApp {
  codename: string;
  name: string;
  description: string;
  tags: ITag[];
  route: string;
  github: string;
}

export class App extends ContentItem {
  static codename = "app";
  name!: Elements.TextElement;
  description!: Elements.RichTextElement;
  tags!: Elements.LinkedItemsElement<Tag>;
  route!: Elements.UrlSlugElement;
  github!: Elements.TextElement;

  getModel(): IApp {
    return {
      name: this.name.value,
      codename: this.system.codename,
      description: this.description.resolveHtml(),
      tags: this.tags.value.map((tag) => tag.getModel()),
      route: this.route.value,
      github: this.github.value,
    };
  }
}
