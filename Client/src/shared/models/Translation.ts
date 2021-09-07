import { ContentItem, Elements } from '@kentico/kontent-delivery';

export class Translation extends ContentItem {
  static codename = "translation";
  content!: Elements.TextElement;
}
