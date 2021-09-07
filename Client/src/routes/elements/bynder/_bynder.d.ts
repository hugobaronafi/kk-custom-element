export type BynderCompactView = {
  open: (options?: Partial<IBynderOptions>) => void;
};

type AssetType = "IMAGE" | "VIDEO" | "AUDIO" | "DOCUMENT";

type Mode = "MultiSelect" | "SingleSelect" | "SingleSelectFile";

type Orientation = "PORTRAIT" | "LANDSCAPE" | "SQUARE";

export type AdditionalInfo = any & Partial<{ selectedFile: any }>;

export interface IBynderOptions {
  onSuccess: (assets: IAsset[], additionalInfo?: AdditionalInfo) => void;
  container: HTMLElement;
  language: string;
  mode: Mode;
  defaultDomain: string;
  assetTypes: AssetType[];
  selectedAssets: string[];
}

export interface IAsset {
  /** Asset Id. */
  id: string;

  /** Asset database Id. */
  databaseId: string;

  /** Asset name. */
  name: string;

  /** Asset description. */
  description: string;

  /** Asset type. Possible values are: IMAGE, DOCUMENT, AUDIO and VIDEO. */
  type: AssetType;

  /** The HTTP URL for this payload.asset. */
  url: any;

  derivatives: { thumbnail: string; webImage: string };

  /** Date created. */
  createdAt: Date;

  /** User who created the payload.asset. */
  createdBy: any;

  /** Date published. */
  publishedAt: Date;

  /** Date updated. */
  updatedAt: Date;

  /** Size of the asset file in bytes. */
  fileSize: number;

  /** Height of the original asset file. */
  height: number;

  /** Width of the original asset file. */
  width: number;

  /** Asset orientation. Possible values are: PORTRAIT, LANDSCAPE, SQUARE. */
  orientation: Orientation;

  /** Whether or not this asset is archived. */
  isArchived: boolean;

  /** Brand ID. */
  brandId: string;

  /** Files associated with the payload.asset. */
  files: any;

  /** Copyright of the payload.asset. */
  copyright: string;

  /** Extensions of the asset file. This can have more than one value in case of a
   * derivative with different extensions.  */
  extensions: string[];

  /** Whether or not this asset is marked as Limited Use. */
  isLimitedUse: boolean;

  /** Whether or not this asset is marked as Public. */
  isPublic: boolean;

  /** Whether or not this asset has a watermark. */
  isWatermarked: boolean;

  /** Tags of the payload.asset. */
  tags: string[];

  /** Asset original URL. */
  originalUrl: any;

  /** Metaproperties of the asset */
  metaproperties: MetapropertyConnection;

  /** Metaproperties of the asset with type `text` and `longtext` */
  textMetaproperties: TextMetaproperty[];
}

type TextMetaproperty = {
  name: string;

  value: string;
};

type MetapropertyConnection = {
  nodes: Metaproperty[];
};

type Metaproperty = {
  id: string;

  name: string;

  /**
   * @param language  Preferred language for this label */
  label(language: string): string;

  type: string;

  zIndex: number;

  options: MetapropertyOption[];

  isApiField: boolean;

  isEditable: boolean;

  isFilterable: boolean;

  isMainfilter: boolean;

  isMultifilter: boolean;

  isMultiselect: boolean;

  isRequired: boolean;

  isSearchable: boolean;
};

type MetapropertyOption = {
  id: string;

  name: string;

  assetCount: number;

  /**
   * @param language Preferred language for this label */
  label(language: string): string;

  displayLabel: string;

  imageUrl: any;

  zIndex: number;

  isActive: boolean;

  isHiddenByDefault: boolean;

  isSelectable: boolean;
};
