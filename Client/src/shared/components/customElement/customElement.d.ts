interface ILanguage {
  id: string;
  codename: string;
}

export interface IAssetDescription {
  language: ILanguage;
  description: string;
}

export interface IAssetDetails {
  id: string;
  descriptions: IAssetDescription[];
  fileName: string;
  imageHeight: number | null;
  imageWidth: number | null;
  name: string;
  size: number;
  thumbnailUrl: string;
  title: string;
  type: string;
  url: string;
}

interface IItem {
  id: string;
  codename: string;
}

interface IVariant {
  id: string;
  codename: string;
}

export interface IContext {
  projectId: string;
  item: IItem;
  variant: IVariant;
}

interface IAssetReference {
  id: string;
}

interface ISelectAssetConfig {
  allowMultiple: boolean;
  fileType: "all" | "images";
}

export interface ICustomElement<TConfig = {}> {
  value: string | null;
  disabled: boolean;
  config: TConfig | null;
  init: (
    callback: (element: ICustomElement<TConfig>, context: IContext) => void
  ) => void;
  setValue: (value: string) => void;
  setHeight: (value: number) => void;
  onDisabledChanged: (callback: (disabled: boolean) => void) => void;
  selectAssets: (config: ISelectAssetConfig) => Promise<IAssetReference[]>;
  getAssetDetails: (assetIds: string[]) => Promise<IAssetDetails[]>;
}
