export interface IQueryRoot {
  search: ISearchConnection;
}

export interface ISearchConnection {
  edges: IProductEdge[];
}

export interface IProductEdge {
  node: IProduct;
}

export interface IProduct {
  id: string;
  name: string;
  variants: IProductVariant[];
}

export interface IProductVariant {
  sku: string;
  images: IImage[];
  priceVariants: IPriceVariant[];
}

export interface IImage {
  url: string;
}

export interface IPriceVariant {
  price: number;
  currency: string;
}
