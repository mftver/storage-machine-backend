/// Provides a model of stock focused on the fact that bins stored in the Storage Machine can be nested in each other,
/// in addition to holding products.
module StorageMachine.Repacking.BinTree

open StorageMachine
open Common

//type MaybePackaged<'a> =
//    | Packaged of 'a
//    | Unpacked of 'a
//
//type Product = {
//    PartNumber : PartNumber
//}

/// Multiple bins can be nested in each other, this forming a "tree" of bins.
type BinTree =
    /// A bin can contain zero or more other (nested) bins and products.
    | Bin of BinIdentifier * List<BinTree>
    /// A product is represented by its part number.
    | PlainProduct of PartNumber
    | PackedProduct of PartNumber

let rec packed (binTree : BinTree) : BinTree = 
    match binTree with
    | Bin (binIdentifier, innerBins) -> Bin(binIdentifier, List.map packed innerBins)
    | PlainProduct product -> PackedProduct (product)
    | PackedProduct _ as alreadyPackaged-> alreadyPackaged


/// Determines how many products are contained in all bins of the given bin tree.
let rec productCount binTree =
    match binTree with
    | Bin (_, productsOrBins) -> List.sumBy productCount productsOrBins
    | PlainProduct _ | PackedProduct _ -> 1
