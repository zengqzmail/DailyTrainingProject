   M S B A M L   `   `   `  ����H  DOnlineBanking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:OnlineBankingOnlineBanking  5      "    OnlineBanking.MakePaymentdup   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -localclr-namespace:OnlineBanking     5        � 	Resources  5
       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $ ProjectTheme.xaml?�6%   %    5      .��5      � .+�5   
   �� -   5        $��FromAccount��5   [   $
��Left=�6   $	��Top=�6.   $��200,200,0,0q�6F   $��40��6n   $	��650��6z   $��25(�6�   .@�5      �� $��25(�6   .��6)   (&Checking Account --- $115.00 available6Q   �� 5      $��25(�6   .��6)   &$Saving Account --- $135.00 available6O   5      �� 5   
   -   5         ��Name$ 	textBlock��5      $
��Left=�6'   $��200,150,0,0q�6B   $
��Wrap=�6W   $��From Account :��6k   $	��Top=�6�   $��25(�6�   �� 5   
   -   5        $ 
textBlock1��5      $
��Left=�6(   $��200,300,0,0q�6C   $
��Wrap=�6X   $��Payee:��6l   $	��Top=�6z   $��25(�6�   �� 5   
   -   5        $ 
textBlock2��5      $
��Left=�6(   $��500,300,0,0q�6C   $
��Wrap=�6X   $V��PFlorida Power and Light (Paid Electronically)
Account Number With Payee:
1234567��6l   $	��Top=�6�   $��25(�6�   �� 5   
   
���   �$
��Left=�6!   $��1��6<   $��200,410,0,0q�6G   
���   �$	��Top=�6k   $	��850��6�   �� 5   
   -   5        $ 
textBlock3��5      $
��Left=�6(   $��200,450,0,0q�6C   $
��Wrap=�6X   $��Amount :��6l   $	��Top=�6|   $��25(�6�   �� 5   
   -   5         ��Name$ textBox��5      $
��Left=�6#   $��40��6>   $��200,500,0,0q�6J    �� TextWrapping$
 Wrap=�6_   $�� ��6s   $	��Top=�6{   $	��150��6�   $��20(�6�   �� 5   
   -   5        $ 
textBlock4��5      $
��Left=�6(   $��200,550,0,0q�6C   $
��Wrap=�6X   $��Set Payment Date :��6l   $	��Top=�6�   $��25(�6�   (  "System.Windows.Controls.DatePicker  5   
   $
��Left=�6   $��200,600,0,0q�60   $	��Top=�6E   �� 5   
   -   5         ��Name$ button��5      $��Continue��6!   $
��Left=�64   $��200,650,0,0q�6O   $	��Top=�6d   $	��100��6|   6�   5       5!      