import { ContentItem, Elements } from '@kentico/kontent-delivery';

import type { ITag, Tag } from "./Tag";

export interface ICustomElement {
  codename: string;
  name: string;
  description: string;
  image: {
    src: string;
    alt: string;
  };
  tags: ITag[];
  route: string;
  github: string;
  options: "demo"[];
  demoConfig: string;
}

export class CustomElement extends ContentItem {
  static codename = "custom_element";
  name!: Elements.TextElement;
  description!: Elements.RichTextElement;
  image!: Elements.AssetsElement;
  tags!: Elements.LinkedItemsElement<Tag>;
  route!: Elements.UrlSlugElement;
  github!: Elements.TextElement;
  options!: Elements.MultipleChoiceElement;
  demo_config!: Elements.TextElement;

  getModel(): ICustomElement {
    return {
      name: this.name.value,
      codename: this.system.codename,
      description: this.description.resolveHtml(),
      image: {
        src: this.image.value[0].url,
        alt: this.image.value[0].description,
      },
      tags: this.tags.value.map((tag) => tag.getModel()),
      route: this.route.value,
      github: this.github.value,
      options: this.options.value.map((option) => option.codename) as any[],
      demoConfig: this.demo_config.value,
    };
  }
}
