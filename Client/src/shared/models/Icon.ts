import { ContentItem, Elements } from '@kentico/kontent-delivery';

export interface IIcon {
  name: string;
  codename: string;
  label: string;
  style: string;
  unicode: string;
  svg: string;
  cssClass: string;
}

export class Icon extends ContentItem {
  static codename = "icon";
  icon!: Elements.CustomElement;

  getModel(): IIcon {
    return {
      ...JSON.parse(this.icon.value).icon,
      codename: this.system.codename,
    };
  }
}
