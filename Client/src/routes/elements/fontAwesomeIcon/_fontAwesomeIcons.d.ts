export interface IFontAwesomeIcons {
  [key: string]: IIcon;
}

type Style = "solid" | "regular" | "light" | "duotone" | "brands";

export interface IIcon {
  changes: string[];
  ligatures: any[];
  search: ISearch;
  styles: Style[];
  unicode: string;
  label: string;
  voted: boolean;
  svg: ISvg;
  free: string[];
}

export interface ISearch {
  terms: string[];
}

export interface ISvg {
  [key: Style]: IStyleSvg;
}

export interface IStyleSvg {
  lastModified: number;
  raw: string;
  viewBox: string[];
  width: number;
  height: number;
  path: string;
}
