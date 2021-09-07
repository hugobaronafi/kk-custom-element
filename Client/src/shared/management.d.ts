interface IContentItemElement {
  id: string;
  type: string;
  name: string;
  codename: string;
  snippet: {
    id: string;
  };
}
export type ElementType =
  | "asset"
  | "snippet"
  | "custom"
  | "date_time"
  | "guidelines"
  | "modular_content"
  | "number"
  | "multiple_choice"
  | "rich_text"
  | "taxonomy"
  | "text"
  | "url_slug";
export interface IContentType {
  id: string;
  name: string;
  codename: string;
  elements: IContentItemElement[];
}

export interface IContentItem {
  id: string;
  name: string;
  codename: string;
  type: {
    id: string;
  };
  external_id: string;
}
