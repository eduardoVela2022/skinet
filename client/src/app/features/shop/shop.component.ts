// Imports
import { Component, inject, OnInit } from '@angular/core';
import { ShopService } from '../../core/service/shop.service';
import { Product } from '../../shared/models/product';
import { MatCard } from '@angular/material/card';
import { ProductItemComponent } from './product-item/product-item.component';
import { MatDialog } from '@angular/material/dialog';
import { FiltersDialogComponent } from './filters-dialog/filters-dialog.component';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatMenu, MatMenuTrigger } from '@angular/material/menu';
import {
  MatListOption,
  MatSelectionList,
  MatSelectionListChange,
} from '@angular/material/list';
import { ShopParams } from '../../shared/models/shopParams';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Pagination } from '../../shared/models/pagination';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-shop',
  standalone: true,
  imports: [
    MatCard,
    ProductItemComponent,
    MatButton,
    MatIcon,
    MatMenu,
    MatSelectionList,
    MatListOption,
    MatMenuTrigger,
    MatPaginator,
    FormsModule,
  ],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss',
})
export class ShopComponent implements OnInit {
  private shopService = inject(ShopService);
  private dialogService = inject(MatDialog);
  products?: Pagination<Product>;
  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'Price: Low-High', value: 'priceAsc' },
    { name: 'Price: High-Low', value: 'priceDesc' },
  ];
  shopParams = new ShopParams();
  pageSizeOptions = [5, 10, 15, 20];

  // When page loads
  ngOnInit(): void {
    this.initializeShop();
  }

  // Gets the products, and their types and brands
  initializeShop() {
    this.shopService.getBrands();
    this.shopService.getTypes();

    // Get all products
    this.getProducts();
  }

  // Get products from service
  getProducts() {
    this.shopService.getProducts(this.shopParams).subscribe({
      next: (response) => (this.products = response), // What to do with the data
      error: (error) => console.log(error), // What to do when an error happens
    });
  }

  // Handles search functionality
  onSearchChange() {
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  // Handles pagination
  handlePageEvent(event: PageEvent) {
    this.shopParams.pageNumber = event.pageIndex + 1;
    this.shopParams.pageSize = event.pageSize;
    this.getProducts();
  }

  // Updates sort property
  onSortChange(event: MatSelectionListChange) {
    // Obtains the selected sort option
    const selectedOption = event.options[0];

    // Sets sort property
    if (selectedOption) {
      this.shopParams.sort = selectedOption.value;
      this.shopParams.pageNumber = 1;
      this.getProducts();
    }
  }

  // Opens dialog window
  openFiltersDialog() {
    const dialogRef = this.dialogService.open(FiltersDialogComponent, {
      minWidth: '500px',
      // Dialog data
      data: {
        selectedBrands: this.shopParams.brands,
        selectedTypes: this.shopParams.types,
      },
    });

    // After dialog window is closed do this
    dialogRef.afterClosed().subscribe({
      next: (result) => {
        if (result) {
          // Get the selected brands and types of the dialog window
          this.shopParams.brands = result.selectedBrands;
          this.shopParams.types = result.selectedTypes;
          this.shopParams.pageNumber = 1;
          // Apply brands and types filters
          this.getProducts();
        }
      },
    });
  }
}
