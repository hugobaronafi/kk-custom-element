import { ContentItem, Elements } from '@kentico/kontent-delivery';

import type { ITag, Tag } from "./Tag";

export interface IWebhook {
  codename: string;
  name: string;
  description: string;
  tags: ITag[];
  github: string;
}

export class Webhook extends ContentItem {
  static codename = "webhook";
  name!: Elements.TextElement;
  description!: Elements.RichTextElement;
  tags!: Elements.LinkedItemsElement<Tag>;
  github!: Elements.TextElement;

  getModel(): IWebhook {
    return {
      name: this.name.value,
      codename: this.system.codename,
      description: this.description.resolveHtml(),
      tags: this.tags.value.map((tag) => tag.getModel()),
      github: this.github.value,
    };
  }
}
