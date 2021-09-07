export interface IQueryRoot {
  products: IProductConnection;
}

export interface IProductConnection {
  edges: IProductEdge[];
}

export interface IProductEdge {
  node: IProduct;
}

export interface IProduct {
  id: string;
  images: IImageConnection;
  title: string;
  descriptionHtml: string;
  variants: IProductVariantConnection;
}

export interface IImageConnection {
  edges: IImagesEdge[];
}

export interface IImagesEdge {
  node: IImage;
}

export interface IImage {
  originalSrc: string;
  transformedSrc: string;
}

export interface IProductVariantConnection {
  edges: IProductVariantEdge[];
}

export interface IProductVariantEdge {
  node: IProductVariant;
}

export interface IProductVariant {
  id: string;
  priceV2: IPriceV2;
}

export interface IPriceV2 {
  amount: string;
  currencyCode: string;
}
