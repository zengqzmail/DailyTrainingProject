   M S B A M L   `   `   `  ����H  DOnlineBanking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:OnlineBankingOnlineBanking  5      #    OnlineBanking.TransferAmount3   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -localclr-namespace:OnlineBanking     5        � 	Resources  5
       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $ ProjectTheme.xaml?�6%   %    5      .��5      � ��5   
   �� 5       �� ImageSource$  /Resources/background4.png��6    j� 
AlignmentX$ Center=�6B    j� 
AlignmentY$	 Top=�6V    j� Stretch$ Uniform=�6g   .+�5   
   �� -   5         ��Name$ 	textBlock��5      $
��Left=�6'   $��
20,400,0,0q�6B   $
��Wrap=�6V   $��From :��6j   $	��Top=�6x   $��20(�6�   �� 5   
   -   5        $
��From��5   s   $
��Left=�6   $	��Top=�6.   $��100,400,0,0q�6F   $��40��6[   $	��900��6g   $��25(�6   .@�5      �� $��25(�6   .��6)   &$Checking Account --- $0.00 available6O   �� 5      $��25(�6   .��6)   &$Saving Account --- $135.00 available6O   5      �� 5   
   -   5        $ 
textBlock1��5      $
��Left=�6(   $��
20,500,0,0q�6C   $
��Wrap=�6W   $
��To :��6k   $	��Top=�6w   $��20(�6�   �� 5   
   -   5        $��To��5   s   $
��Left=�6   $	��Top=�6.   $��100,500,0,0q�6F   $��40��6[   $	��900��6g   $��25(�6}   .@�5      �� $��25(�6   .��6)   &$Checking Account --- $0.00 available6O   �� 5      $��25(�6   .��6)   &$Saving Account --- $135.00 available6O   5      �� 5   
   -   5        $ 
textBlock2��5      $
��Left=�6(   $��
20,600,0,0q�6C   $
��Wrap=�6W   $��Amount :��6k   $	��Top=�6{   $��15(�6�   �� 5    
   -   5         ��Name$ textBox��5       $
��Left=�6#   $��20��6>   $��100,600,0,0q�6J   	 �� TextWrapping$
	 Wrap=�6_   $�� ��6s   $	��Top=�6{   $	��120��6�   �� 5!   
   -   5        
 ��Name$
 button��5!      $��Continue��6!   $
��Left=�64   $��250,650,0,0q�6O   $	��Top=�6d   $	��100��6|   6�   �� 5"   
   -   5        $ 
textBlock3��5"      $
��Left=�6(   $��
20,350,0,0q�6C   $
��Wrap=�6W   $��Transfer Amount��6k   $	��Top=�6�   $��20(�6�   �� 5#   
   -	   5        $ 
textBlock4��5#      $
��Left=�6(   $��
20,300,0,0q�6C   $
��Wrap=�6W   $<��6Please fill in the information for transfering amount.��6k   $	��Top=�6�   $��30(�6�   
����  �6�   5%      5&      